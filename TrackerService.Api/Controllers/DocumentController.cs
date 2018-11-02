using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using TrackerService.Api.Filters;
using TrackerService.Data.Contracts;

namespace TrackerService.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/document")]
    public class DocumentController : ControllerBase
    {
        private static readonly FormOptions defaultFormOptions = new FormOptions();
        private readonly ILogger<DocumentController> logger;
        private readonly IDocumentStorageRepository storageRepository;

        public DocumentController(ILogger<DocumentController> logger, IRepositoryFactory repositoryFactory)
        {
            this.logger = logger;
            storageRepository = repositoryFactory.CreateDocumentStorageRepository();
        }

        [HttpPost]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> UploadAsync()
        {
            if (!IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            var formAccumulator = new KeyValueAccumulator();

            var boundary = GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, Request.Body);
            var section = await reader.ReadNextSectionAsync();
            var filesUploaded = new List<string>();

            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (HasFileContentDisposition(contentDisposition))
                    {
                        logger.LogInformation($"The file name of uploaded file is: {contentDisposition.FileName}");
                        await storageRepository.SaveDocument($"{Guid.NewGuid()}_{contentDisposition.FileName}", section.Body);
                    }
                }
                else if (HasFormDataContentDisposition(contentDisposition))
                {
                    var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                    var encoding = GetEncoding(section);
                    using (var streamReader = new StreamReader(section.Body, encoding, true, 1024, true))
                    {
                        var value = await streamReader.ReadToEndAsync();
                        if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.Empty;
                        }

                        formAccumulator.Append(key.Value, value);

                        if (formAccumulator.ValueCount > defaultFormOptions.ValueCountLimit)
                        {
                            throw new InvalidDataException($"Form key count limit {defaultFormOptions.ValueCountLimit} exceeded");
                        }
                    }
                }

                section = await reader.ReadNextSectionAsync();
            }

            return Ok(new {Message = "File upload successful", Path = filesUploaded});
        }

        private static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            if (!boundary.HasValue || string.IsNullOrWhiteSpace(boundary.Value))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded");
            }

            return boundary.Value;
        }

        private static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType) &&
                   contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            return contentDisposition != null &&
                   contentDisposition.DispositionType.Equals("form-data") &&
                   string.IsNullOrEmpty(contentDisposition.FileName.Value) &&
                   string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            return contentDisposition != null &&
                   contentDisposition.DispositionType.Equals("form-data") &&
                   (!string.IsNullOrEmpty(contentDisposition.FileName.Value) || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out var mediaType);

            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }

            return mediaType.Encoding;
        }
    }
}
