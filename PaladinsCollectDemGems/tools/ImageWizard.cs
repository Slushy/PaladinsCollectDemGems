using PaladinsCollectDemGems.game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PaladinsCollectDemGems.tools
{
	/// <summary>
	/// Static utility class to provide common image functions
	/// </summary>
	public class ImageWizard
	{
		private const int THUMBNAIL_SIZE_X = 455;
		private const int THUMBNAIL_SIZE_Y = 256;

		/// <summary>
		/// Loads the image into a bitmap
		/// </summary>
		/// <param name="pathFromResources"></param>
		/// <returns></returns>
		public static Bitmap LoadImage(string pathFromResources)
		{
			string imagePath = Path.Combine("..\\..\\resources", pathFromResources);
			return (Bitmap)Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), imagePath));
		}

		/// <summary>
		/// Takes a picture of the current window and retrieves the thumbnail sized version before returning
		/// </summary>
		/// <param name="window"></param>
		/// <returns>thumbnail of the picture of the window</returns>
		public static Bitmap GetThumbnailOfWindow(Window window)
		{
			return (Bitmap)window.CaptureImage()
						.GetThumbnailImage(THUMBNAIL_SIZE_X, THUMBNAIL_SIZE_Y, () => false, IntPtr.Zero);
		}

		/// <summary>
		/// Compares two images to each other and returns the percentage (0-100)
		/// </summary>
		/// <param name="image1"></param>
		/// <param name="image2"></param>
		/// <returns></returns>
		public static double getPercentagePixelMatch(Bitmap image1, Bitmap image2)
		{
			// Get the hashes for each image
			List<bool> image1Hash = GetImageHash(image1);
			List<bool> image2Hash = GetImageHash(image2);

			// Return the percentage they match
			double pixelCount = image1.Height * image1.Width;
			int numOfMatchingPixels = image1Hash.Zip(image2Hash, (i, j) => i == j).Count(eq => eq);
			return numOfMatchingPixels / pixelCount * 100.0;
		}

		/// <summary>
		/// Takes an image and generates a brightness image hash for comparing. Easy to do, not always accurate.
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		private static List<bool> GetImageHash(Bitmap image)
		{
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
