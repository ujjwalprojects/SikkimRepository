using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace SikkimRepository.Web.Utility
{
    public static class FileTypeCheck
    {
        public static string IsValidFile(this HttpPostedFileBase postedFile, string fileType)
        {
            if (postedFile == null)
            {
                return "Select a file";
            }
            switch (fileType)
            {
                case "Image":
                    {
                        //-------------------------------------------
                        //  Check the image mime types
                        //-------------------------------------------
                        if (postedFile.ContentType.ToLower() != "image/jpg" &&
                                    postedFile.ContentType.ToLower() != "image/jpeg" &&
                                    postedFile.ContentType.ToLower() != "image/pjpeg" &&
                                    postedFile.ContentType.ToLower() != "image/x-png" &&
                                    postedFile.ContentType.ToLower() != "image/png")
                        {
                            return "Please select an image file";
                        }

                        //-------------------------------------------
                        //  Check the image extension
                        //-------------------------------------------
                        if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                            && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                            && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
                        {
                            return "Please select an image file";
                        }
                        try
                        {
                            if (!postedFile.InputStream.CanRead)
                            {
                                return "Please select an image file";
                            }

                            if (postedFile.ContentLength > 10485760)
                            {
                                return "Please select an image with maximum size of 10MB.";
                            }

                            byte[] buffer = new byte[512];
                            postedFile.InputStream.Read(buffer, 0, 512);
                            string content = System.Text.Encoding.UTF8.GetString(buffer);
                            if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                            {
                                return "Please select an image file";
                            }
                        }
                        catch (Exception)
                        {
                            return "Please select an image file";
                        }

                        try
                        {
                            using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                            {
                            }
                        }
                        catch (Exception)
                        {
                            return "Please select an image file";
                        }
                        finally
                        {
                            postedFile.InputStream.Position = 0;
                        }

                        return "Success";
                    }
                case "Video":
                    {
                        //-------------------------------------------
                        //  Check the video mime types
                        //-------------------------------------------
                        if (postedFile.ContentType.ToLower() != "video/mp4")
                        {
                            return "Please select a video file";
                        }

                        //-------------------------------------------
                        //  Check the video extension
                        //-------------------------------------------
                        if (Path.GetExtension(postedFile.FileName).ToLower() != ".mp4")
                        {
                            return "Please select a video file";
                        }
                        try
                        {
                            if (!postedFile.InputStream.CanRead)
                            {
                                return "Please select a video file";
                            }

                            if (postedFile.ContentLength > 104857600)
                            {
                                return "Please select a video with maximum size of 100 MB.";
                            }

                            byte[] buffer = new byte[512];
                            postedFile.InputStream.Read(buffer, 0, 512);
                            string content = System.Text.Encoding.UTF8.GetString(buffer);
                            if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                            {
                                return "Please select a video file";
                            }
                        }
                        catch (Exception)
                        {
                            return "Please select a video file";
                        }

                        return "Success";
                    }
                case "File":
                    {
                        //-------------------------------------------
                        //  Check the file mime types
                        //-------------------------------------------
                        if (postedFile.ContentType.ToLower() != "application/pdf" &&
                            postedFile.ContentType.ToLower() != "text/plain" &&
                            postedFile.ContentType.ToLower() != "application/zip" &&
                            postedFile.ContentType.ToLower() != "application/vnd.ms-excel" &&
                            postedFile.ContentType.ToLower() != "application/msword" &&
                            postedFile.ContentType.ToLower() != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" &&
                            postedFile.ContentType.ToLower() != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" &&
                            postedFile.ContentType.ToLower() != "application/vnd.openxmlformats-officedocument.presentationml.presentation" &&
                            postedFile.ContentType.ToLower() != "application/vnd.ms-powerpoint")
                        {
                            return "Please select a valid file";
                        }

                        //-------------------------------------------
                        //  Check the file extension
                        //-------------------------------------------
                        if (Path.GetExtension(postedFile.FileName).ToLower() != ".pdf" &&
                            Path.GetExtension(postedFile.FileName).ToLower() != ".txt" &&
                            Path.GetExtension(postedFile.FileName).ToLower() != ".zip" &&
                            //Path.GetExtension(postedFile.FileName).ToLower() != ".xls" &&
                            //Path.GetExtension(postedFile.FileName).ToLower() != ".xlsx" &&
                            Path.GetExtension(postedFile.FileName).ToLower() != ".doc" &&
                            Path.GetExtension(postedFile.FileName).ToLower() != ".docx" &&
                            Path.GetExtension(postedFile.FileName).ToLower() != ".ppt" &&
                            Path.GetExtension(postedFile.FileName).ToLower() != ".pptx")
                        {
                            return "Please select a valid file";
                        }
                        try
                        {
                            if (!postedFile.InputStream.CanRead)
                            {
                                return "Please select a valid file";
                            }

                            if (postedFile.ContentLength > 10485760)
                            {
                                return "Please select a file with maximum size of 10MB.";
                            }

                            byte[] buffer = new byte[512];
                            postedFile.InputStream.Read(buffer, 0, 512);
                            string content = System.Text.Encoding.UTF8.GetString(buffer);
                            
                            if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                            {
                                return "Please select a valid file";
                            }
                        }
                        catch (Exception)
                        {
                            return "Please select a valid file";
                        }

                        return "Success";
                    }
                default:
                    return "Invalid file";
            }

        }
    }

}