/*
 * Created by SharpDevelop.
 * User: user
 * Date: 3/11/2009
 * Time: 9:34 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing;
using MagickNet;

namespace ImageHeaven
{
	/// <summary>
	/// Description of ImageCC.
	/// </summary>
	public abstract class ImageCC
	{
		public ImageCC()
		{
		}
		public abstract MagickNet.Image Crop(MagickNet.Image prmImage,Rectangle prmRect);
		public abstract MagickNet.Image Crop(MagickNet.Image prmImage,MagickNet.Geometry prmRect);
		public abstract MagickNet.Image Crop(string imageFileName,Rectangle prmRect);
		public abstract MagickNet.Image Rotate(MagickNet.Image prmImage,double prmDegree);
		public abstract MagickNet.Image Skew(MagickNet.Image prmImage,double xAngle,double yAngle);
		public abstract MagickNet.Image RemoveNoise(MagickNet.Image prmImage);
		public abstract MagickNet.Image RemoveNoise(string prmFileName);
		public abstract MagickNet.Image AutoCrop(MagickNet.Image prmImage);
        public abstract MagickNet.Image AutoCrop(string prmFileName);
		public abstract MagickNet.Image Despacle(MagickNet.Image prmImage);
		public abstract MagickNet.Image Zoom(MagickNet.Image prmImage,Size prmSize);
		public abstract MagickNet.Image Zoom(string prmFileName,Size udtSize);
		public abstract MagickNet.Image Rotate(string prmFileName, double prmDegree);
	}
	
}
