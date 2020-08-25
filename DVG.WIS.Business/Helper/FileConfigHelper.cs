using DVG.WIS.Utilities.Logs;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DVG.WIS.Business
{
    public class FileConfigHelper
    {
        public async static Task<string> GetFileContentAsync(string filePath)
        {
            var runCount = 1;

            while (runCount < 4)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        using (var fileStreamReader = File.OpenText(filePath))
                        {
                            return await fileStreamReader.ReadToEndAsync();
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException();
                    }
                }
                catch (IOException ex)
                {
                    if (runCount == 3 || ex.HResult != -2147024864)
                    {
                        throw;
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, runCount)));
                        runCount++;
                    }
                }
            }

            return null;
        }

        public static string GetFileContent(string filePath)
        {
            var runCount = 1;

            while (runCount < 4)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        using (var fileStreamReader = File.OpenText(filePath))
                        {
                            return fileStreamReader.ReadToEnd();
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException();
                    }
                }
                catch (IOException ex)
                {
                    if (runCount == 3 || ex.HResult != -2147024864)
                    {
                        throw;
                    }
                    else
                    {
                        Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, runCount)));
                        runCount++;
                    }
                }
            }

            return "";
        }

        public static string ReadFileConfig(string filename, PhysicalFileProvider fileProvider = null)
        {
            var result = "";
            try
            {
                fileProvider = fileProvider ?? new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Configs"));

                using (var streamReader = new StreamReader(fileProvider.GetFileInfo(filename).CreateReadStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, "ReadFileConfig_" + filename);
            }
            return result;
        }
    }
}