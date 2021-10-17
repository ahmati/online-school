using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using OnlineSchool.Core.Documents_.Service;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSchool.Site.Quartz.Jobs
{
    /// <summary>
    /// This job clears temporary files that might have been left over (temp files older than 24h)
    /// </summary>
    [DisallowConcurrentExecution]
    public class ClearTempFilesJob : IJob
    {
        private readonly ILogger<ClearTempFilesJob> _logger;
        public IDocumentService DocumentService { get; set; }
        public IWebHostEnvironment _webHostEnvironment { get; set; }

        public ClearTempFilesJob(ILogger<ClearTempFilesJob> logger, IWebHostEnvironment webHostEnvironment, IDocumentService documentService)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            DocumentService = documentService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            ClearTempStudentDocuments();
            ClearTempTeacherDocuments();
            ClearTempTeacherSubjectMaterials();
            ClearTempTeacherSubjectLessonMaterials();
            return Task.CompletedTask;
        }

        private void ClearTempStudentDocuments()
        {
            try
            {
                var rootDir = $"{_webHostEnvironment.ContentRootPath}\\private_uploads\\documents\\students";
                var dirs = Directory.EnumerateDirectories(rootDir);

                foreach (var dir in dirs)
                {
                    var tempFiles = Directory.EnumerateFiles($"{dir}\\temp");
                    foreach (var tempFile in tempFiles)
                    {
                        var fileInfo = new FileInfo(tempFile);
                        var hoursOld = DateTime.Now.Subtract(fileInfo.LastAccessTime).TotalHours;
                        if (hoursOld >= 24)
                            File.Delete(tempFile);
                    }
                }
            }
            catch(Exception e) { }
        }

        private void ClearTempTeacherDocuments()
        {
            try
            {
                var rootDir = $"{_webHostEnvironment.ContentRootPath}\\private_uploads\\documents\\teachers";
                var dirs = Directory.EnumerateDirectories(rootDir);

                foreach (var dir in dirs)
                {
                    var tempFiles = Directory.EnumerateFiles($"{dir}\\temp");
                    foreach (var tempFile in tempFiles)
                    {
                        var fileInfo = new FileInfo(tempFile);
                        var hoursOld = DateTime.Now.Subtract(fileInfo.LastAccessTime).TotalHours;
                        if (hoursOld >= 24)
                            File.Delete(tempFile);
                    }
                }
            }
            catch(Exception e) { }
        }

        private void ClearTempTeacherSubjectMaterials()
        {
            try
            {
                var rootDir = $"{_webHostEnvironment.ContentRootPath}\\private_uploads\\materials\\teachersubject";
                var dirs = Directory.EnumerateDirectories(rootDir);

                foreach (var dir in dirs)
                {
                    var tempFiles = Directory.EnumerateFiles($"{dir}\\temp");
                    foreach (var tempFile in tempFiles)
                    {
                        var fileInfo = new FileInfo(tempFile);
                        var hoursOld = DateTime.Now.Subtract(fileInfo.LastAccessTime).TotalHours;
                        if (hoursOld >= 24)
                            File.Delete(tempFile);
                    }
                }
            }
            catch(Exception e) { }
        }

        private void ClearTempTeacherSubjectLessonMaterials()
        {
            try
            {
                var rootDir = $"{_webHostEnvironment.ContentRootPath}\\private_uploads\\materials\\teachersubject";
                var dirs = Directory.EnumerateDirectories(rootDir);

                foreach (var dir in dirs)
                {
                    var lessonDirs = Directory.EnumerateDirectories($"{dir}\\lessons");
                    foreach (var lessonDir in lessonDirs)
                    {
                        var tempFiles = Directory.EnumerateFiles($"{lessonDir}\\temp");

                        foreach (var tempFile in tempFiles)
                        {
                            var fileInfo = new FileInfo(tempFile);
                            var hoursOld = DateTime.Now.Subtract(fileInfo.LastAccessTime).TotalHours;
                            if (hoursOld >= 24)
                                File.Delete(tempFile);
                        }
                    }
                }
            }
            catch(Exception e) { }
        }
    }
}
