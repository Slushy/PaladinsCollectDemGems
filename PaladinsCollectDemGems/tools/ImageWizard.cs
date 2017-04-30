using PaladinsCollectDemGems.game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PaladinsCollectDemGems.tools
{
	public class ImageWizard
	{
		private const int IMAGE_SIZE = 256;
		private const int PIXEL_COUNT = IMAGE_SIZE * IMAGE_SIZE;

		public static Bitmap LoadImage(string pathFromResources) {
			string imagePath = Path.Combine("..\\..\\resources", pathFromResources);
			return (Bitmap)Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), imagePath));
		}

		public static Bitmap GetThumbnailOfWindow(Window window) {
			return (Bitmap)window.CaptureImage()
						.GetThumbnailImage(IMAGE_SIZE, IMAGE_SIZE, () => false, IntPtr.Zero);
		}

		public static double getPercentagePixelMatch(Bitmap image1, Bitmap image2) {
			// Get the hashes for each image
			List<bool> image1Hash = GetImageHash(image1);
			List<bool> image2Hash = GetImageHash(image2);

			// Return the percentage they match
			double pixelCount = image1.Height * image1.Width;
			int numOfMatchingPixels = image1Hash.Zip(image2Hash, (i, j) => i == j).Count(eq => eq);
			return numOfMatchingPixels / pixelCount * 100.0;
		}

		private static List<bool> GetImageHash(Bitmap image) {
			List<bool> imageHash = new List<bool>();
			for (int j = 0; j < image.Height; j++)
			{
				for (int i = 0; i < image.Width; i++)
				{
					//reduce colors to true / false                
					imageHash.Add(image.GetPixel(i, j).GetBrightness() < 0.5f);
				}
			}
			return imageHash;
		}
	}
}
