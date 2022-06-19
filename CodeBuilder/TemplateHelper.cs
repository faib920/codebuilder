// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using CodeBuilder.Core;
using CodeBuilder.Core.Template;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeBuilder
{
    internal class TemplateHelper
    {
        /// <summary>
        /// 将模板压缩成包。
        /// </summary>
        /// <param name="hosting"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        internal static byte[] ZipPackage(IDevHosting hosting, TemplateDefinition definition)
        {
            using (var memory = new MemoryStream())
            using (var archive = new ZipOutputStream(memory))
            {
                if (definition.Extension?.Common?.Count > 0)
                {
                    PutExtension(hosting, "common", definition.Extension.Common, archive);
                }

                if (definition.Extension?.Profile?.Count > 0)
                {
                    PutExtension(hosting, "profile", definition.Extension.Profile, archive);
                }

                if (definition.Extension?.Schema?.Count > 0)
                {
                    PutExtension(hosting, "schema", definition.Extension.Schema, archive);
                }

                var pathData = definition.TemplateDirectory.Replace(Path.Combine(hosting.WorkPath, "templates\\"), string.Empty).Split('\\');
                var rootEntryName = "template." + pathData[0] + "/";
                archive.PutNextEntry(new ZipEntry(rootEntryName));

                PutFile(archive, rootEntryName + new FileInfo(definition.ConfigFileName).Name, definition.ConfigFileName);

                rootEntryName += pathData[1] + "/";

                ZipDirectory(rootEntryName, definition.TemplateDirectory, null, archive);

                archive.Finish();

                return memory.ToArray();
            }
        }

        private static void PutExtension(IDevHosting hosting, string name, List<string> files, ZipOutputStream archive)
        {
            var entry = new ZipEntry("extension." + name + "/");
            archive.PutNextEntry(entry);

            foreach (var file in files)
            {
                var filePath = Path.Combine(hosting.WorkPath, "extensions", name, file);
                if (File.Exists(filePath))
                {
                    PutFile(archive, "extension." + name + "/" + file, filePath);
                }
            }
        }

        private static void ZipDirectory(string entryName, string root, string path, ZipOutputStream archive)
        {
            var currEntryName = string.IsNullOrEmpty(path) ? entryName : entryName + path.Replace(root + "\\", string.Empty) + "/";
            archive.PutNextEntry(new ZipEntry(currEntryName));

            foreach (string filePath in Directory.GetFiles(path ?? root))
            {
                PutFile(archive, currEntryName + new FileInfo(filePath).Name, filePath);
            }

            foreach (string folder in Directory.GetDirectories(path ?? root))
            {
                ZipDirectory(entryName, root, folder, archive);
            }
        }

        private static void PutFile(ZipOutputStream archive, string entryName, string filePath)
        {
            var crc = new Crc32();

            var entry = new ZipEntry(entryName);

            var buffer = File.ReadAllBytes(filePath);
            entry.DateTime = DateTime.Now;
            entry.Size = buffer.Length;
            crc.Reset();
            crc.Update(buffer);
            entry.Crc = crc.Value;
            archive.PutNextEntry(entry);

            archive.Write(buffer, 0, buffer.Length);
        }

        internal static void UnzipPackage(IDevHosting hosting, ITemplateProvider provider, string fileName)
        {
            UnzipPackage(hosting, provider, File.ReadAllBytes(fileName));
        }

        /// <summary>
        /// 从包中解压模板。
        /// </summary>
        /// <param name="hosting"></param>
        /// <param name="provider"></param>
        /// <param name="bytes"></param>
        internal static void UnzipPackage(IDevHosting hosting, ITemplateProvider provider, byte[] bytes)
        {
            using (var memory = new MemoryStream(bytes))
            using (var archive = new ZipInputStream(memory))
            {
                ZipEntry entry;
                while ((entry = archive.GetNextEntry()) != null)
                {
                    var directoryName = Path.GetDirectoryName(entry.Name);
                    var fileName = Path.GetFileName(entry.Name);
                    var path = string.Empty;

                    if (directoryName.StartsWith("extension.common"))
                    {
                        path = Path.Combine(hosting.WorkPath, "extensions", "common");
                    }
                    else if (directoryName.StartsWith("extension.profile"))
                    {
                        path = Path.Combine(hosting.WorkPath, "extensions", "profile");
                    }
                    else if (directoryName.StartsWith("extension.schema"))
                    {
                        path = Path.Combine(hosting.WorkPath, "extensions", "schema");
                    }
                    else if (directoryName.StartsWith("template"))
                    {
                        if (directoryName.IndexOf(".") == -1)
                        {
                            path = provider.WorkDir;
                        }
                        else
                        {
                            var tname = directoryName.Split('\\')[0];
                            path = Path.Combine(hosting.WorkPath, "templates", tname.Substring(tname.IndexOf(".") + 1));
                        }
                    }

                    directoryName = directoryName.IndexOf('\\') != -1 ? string.Join("\\", directoryName.Split('\\').Skip(1)) : null;

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        path = Path.Combine(path, directoryName);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        var fullName = Path.Combine(path, fileName);

                        using (var writer = File.Create(fullName))
                        {
                            var bufferSize = 2048;
                            var buffer = new byte[2048];

                            while ((bufferSize = archive.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                writer.Write(buffer, 0, bufferSize);
                            }
                        }
                    }
                }
            }
        }
    }
}
