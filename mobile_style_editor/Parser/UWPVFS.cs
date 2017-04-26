#if __UWP__
using ICSharpCode.SharpZipLib.VirtualFileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mobile_style_editor
{
	/* Universal Windows Platform Virtual File System */
	public class UWPVFS : ICSharpCode.SharpZipLib.VirtualFileSystem.IVirtualFileSystem
	{
		
        class ElementInfo : IVfsElement
        {
            protected FileSystemInfo Info;
            public ElementInfo(FileSystemInfo info)
            {
                Info = info;
            }

            public string Name
            {
                get { return Info.Name; }
            }

            public bool Exists
            {
                get { return Info.Exists; }
            }

            public ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes Attributes
            {
                get
                {
                    ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes attrs = 0;
                    if (Info.Attributes.HasFlag(System.IO.FileAttributes.Normal)) attrs |= ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes.Normal;
                    if (Info.Attributes.HasFlag(System.IO.FileAttributes.ReadOnly)) attrs |= ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes.ReadOnly;
                    if (Info.Attributes.HasFlag(System.IO.FileAttributes.Hidden)) attrs |= ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes.Hidden;
                    if (Info.Attributes.HasFlag(System.IO.FileAttributes.Directory)) attrs |= ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes.Directory;
                    if (Info.Attributes.HasFlag(System.IO.FileAttributes.Archive)) attrs |= ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes.Archive;

                    return attrs;
                }
            }

            public DateTime CreationTime
            {
                get { return Info.CreationTime; }
            }

            public DateTime LastAccessTime
            {
                get { return Info.LastAccessTime; }
            }

            public DateTime LastWriteTime
            {
                get { return Info.LastWriteTime; }
            }
        }
        class DirInfo : ElementInfo, IDirectoryInfo
        {
            public DirInfo(DirectoryInfo dInfo) : base(dInfo)
            {
            }
        }
        class FilInfo : ElementInfo, IFileInfo
        {
            protected FileInfo FInfo { get { return (FileInfo)Info; } }
            public FilInfo(FileInfo fInfo) : base(fInfo)
            {
            }
            
            public long Length
            {
                get { return FInfo.Length; }
            }
        }

        public System.Collections.Generic.IEnumerable<string> GetFiles(string directory)
        {
            return Directory.GetFiles(directory);
        }

        public System.Collections.Generic.IEnumerable<string> GetDirectories(string directory)
        {
            return Directory.GetDirectories(directory);
        }

        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public IDirectoryInfo GetDirectoryInfo(string directoryName)
        {
            return new DirInfo(new DirectoryInfo(directoryName));
        }

        public IFileInfo GetFileInfo(string filename)
        {
            return new FilInfo(new FileInfo(filename));
        }

        public void SetLastWriteTime(string name, DateTime dateTime)
        {
            File.SetLastWriteTime(name, dateTime);
        }

        public void SetAttributes(string name, ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes attributes)
        {
            System.IO.FileAttributes attrs = 0;
            if (attributes.HasFlag(ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes.Normal)) attrs |= System.IO.FileAttributes.Normal;
            if (attributes.HasFlag(ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes.ReadOnly)) attrs |= System.IO.FileAttributes.ReadOnly;
            if (attributes.HasFlag(ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes.Hidden)) attrs |= System.IO.FileAttributes.Hidden;
            if (attributes.HasFlag(ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes.Directory)) attrs |= System.IO.FileAttributes.Directory;
            if (attributes.HasFlag(ICSharpCode.SharpZipLib.VirtualFileSystem.FileAttributes.Archive)) attrs |= System.IO.FileAttributes.Archive;
            File.SetAttributes(name, attrs);
        }

        public void CreateDirectory(string directory)
        {
            Directory.CreateDirectory(directory);
        }

        public string GetTempFileName()
        {
            return Path.GetTempFileName();
        }

        public void CopyFile(string fromFileName, string toFileName, bool overwrite)
        {
            File.Copy(fromFileName, toFileName, overwrite);
        }

        public void MoveFile(string fromFileName, string toFileName)
        {
            File.Move(fromFileName, toFileName);
        }

        public void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }

        public VfsStream CreateFile(string filename)
        {
            return new VfsProxyStream(new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read), filename);
        }

        public VfsStream OpenReadFile(string filename)
        {
            return new VfsProxyStream(new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.Read), filename);
        }

        public VfsStream OpenWriteFile(string filename)
        {
            return new VfsProxyStream(File.OpenWrite(filename), filename);
        }

        public string CurrentDirectory
        {
            get { return Windows.Storage.ApplicationData.Current.LocalFolder.Path; }
        }

        public char DirectorySeparatorChar
        {
            get { return Path.DirectorySeparatorChar; }
        }
    }
}
#endif
