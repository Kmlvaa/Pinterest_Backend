using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Pinterest.Services
{
	public class FileService
	{
		public string AddFile(IFormFile file)
		{
			string path = Path.Combine(Directory.GetCurrentDirectory(), "Images");

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			var fileInfo = new FileInfo(file.FileName);
			var fileName = fileInfo.Name + Guid.NewGuid().ToString() + fileInfo.Extension;

			string fileNameWithPath = Path.Combine(path, fileName);

			using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			return fileName;
		}
		public void DeleteFile(string fileName)
		{
			string path = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Images", fileName));

			if (!File.Exists(path)) return;

			File.Delete(path);
		}
	}
}
