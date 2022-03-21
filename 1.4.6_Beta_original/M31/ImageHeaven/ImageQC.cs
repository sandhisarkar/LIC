/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 12/3/2008
 * Time: 11:08 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing;
using MagickNet;
using System.Windows.Forms;
using NovaNet.Utils;

namespace ImageHeaven
{
	/// <summary>
	/// Description of ImageQC.
	/// </summary>
	public class ImageQC: ImageCC
	{
		private MagickNet.Image imgManipulation;
		//private System.Drawing.Image manupaltedBMP;
		private string err=null;
		
		public ImageQC()
		{
			MagickNet.Magick.Init();
		}
		public override MagickNet.Image AutoCrop(MagickNet.Image prmImage)
		{
			try
			{
				imgManipulation=new MagickNet.Image(prmImage);
				imgManipulation.Trim();
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
        public override MagickNet.Image AutoCrop(String prmFileName)
        {
            try
            {
                imgManipulation = new MagickNet.Image(prmFileName);
                imgManipulation.Trim();
                //manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
            }
            catch (MagickException ex)
            {
                MessageBox.Show(Constants.IMAGE_ERROR, "B'Zer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                err = ex.Message;
            }
            return imgManipulation;
        }
		public override MagickNet.Image Crop(MagickNet.Image prmImage, MagickNet.Geometry prmRect)
		{
			try
			{
				//MagickNet.Magick.Init();
				imgManipulation=new MagickNet.Image(prmImage);
				imgManipulation.Crop(prmRect);
				//imgManipulation.sCrop(prmRect);
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
		public override MagickNet.Image Crop(MagickNet.Image prmImage, Rectangle prmRect)
		{
			try
			{
				//MagickNet.Magick.Init();
				//imgManipulation=new MagickNet.Image(prmImage);
				prmImage.Crop(prmRect);			
				//MagickNet.Magick.Term();
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return prmImage;
		}
		public override MagickNet.Image Crop(string prmFileName, Rectangle prmRect)
		{
			try
			{
				MagickNet.Magick.Init();
				imgManipulation=new MagickNet.Image(prmFileName);
				MagickNet.Geometry gm=new Geometry(prmRect);
				imgManipulation.Crop(gm);			
				//MagickNet.Magick.Term();
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
		public override MagickNet.Image Despacle(MagickNet.Image prmImage)
		{
			try
			{
				imgManipulation=new MagickNet.Image(prmImage);
				imgManipulation.Despeckle();
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
		public override MagickNet.Image RemoveNoise(MagickNet.Image prmImage)
		{
			try
			{
				imgManipulation=new MagickNet.Image(prmImage);
				imgManipulation.ReduceNoise();
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
		
		public override MagickNet.Image RemoveNoise(string prmFileName)
		{
			try
			{
				imgManipulation=new MagickNet.Image(prmFileName);
				imgManipulation.ReduceNoise();
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
		public override MagickNet.Image Rotate(MagickNet.Image prmImage, double prmDegree)
		{
			try
			{
				imgManipulation=new MagickNet.Image(prmImage);
				imgManipulation.Rotate(prmDegree);
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
		public override MagickNet.Image Rotate(string prmFileName, double prmDegree)
		{
			try
			{
				imgManipulation=new MagickNet.Image(prmFileName);
				imgManipulation.Rotate(prmDegree);
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
		public override MagickNet.Image Skew(MagickNet.Image prmImage, double xAngle, double yAngle)
		{
			try
			{
				MagickNet.Magick.Init();
				imgManipulation=new MagickNet.Image(prmImage);
				imgManipulation.Shear(xAngle,yAngle);
				MagickNet.Magick.Term();
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
		public override MagickNet.Image Zoom(MagickNet.Image prmImage,Size udtSize)
		{
			try
			{
				MagickNet.Magick.Init();
				imgManipulation=new MagickNet.Image(prmImage);
				imgManipulation.Zoom(udtSize);
				MagickNet.Magick.Term();
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
		public override MagickNet.Image Zoom(string prmFileName,Size udtSize)
		{
			try
			{
				MagickNet.Magick.Init();
				imgManipulation=new MagickNet.Image(prmFileName);
				imgManipulation.Zoom(udtSize);
				MagickNet.Magick.Term();
				//manupaltedBMP=MagickNet.Image.ToBitmap(imgManipulation);
			}
			catch(MagickException ex)
			{
				MessageBox.Show(Constants.IMAGE_ERROR,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
				err=ex.Message;
			}
			return imgManipulation;
		}
	}
}
