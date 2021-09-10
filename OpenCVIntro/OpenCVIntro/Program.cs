using OpenCvSharp;
using System;

namespace OpenCVIntro
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Mat foreground = Cv2.ImRead(@"C:\Users\Chris\Pictures\FerrariImage.jpg");

            Mat background = Cv2.ImRead(@"C:\Users\Chris\Pictures\MarioBackground.jpg");

            background = background.Resize(new OpenCvSharp.Size(foreground.Width, foreground.Height));


            Mat foregroundHSV = foreground.CvtColor(ColorConversionCodes.BGR2HSV);


            Mat mask = foregroundHSV.InRange(new Scalar(45, 60, 60), new Scalar(65, 255, 255)); //Scalar: lower bounds and upper bounds

            Mat invertMask = new Mat();
            Cv2.BitwiseNot(mask, invertMask);

            Mat finalForeground = new Mat();

            foreground.CopyTo(finalForeground, mask);

            //Mat finalBackground = new Mat();

            //background.CopyTo(finalBackground, mask);

            //Mat finalImage = new Mat();

            //Cv2.BitwiseOr(finalBackground, finalForeground, finalImage);



            Cv2.NamedWindow("Window", WindowMode.AutoSize);

            //MouseEvent @event, int x, int y, MouseEvent flags, IntPtr, userdata


            Cv2.SetMouseCallback("Window", (evnt, x, y, flags, _) =>
            {
                if (x < 0 || x >= foregroundHSV.Width || y < 0 || y >= foregroundHSV.Height)
                {
                    return;
                }

                Vec3b hsv = foregroundHSV.At<Vec3b>(y, x); //row then column
                Console.WriteLine($"H: {hsv.Item0}, S: {hsv.Item1}, V: {hsv.Item2}"); // Reads the HSV values from an area

                //Read the green values and write it down

            });

            VideoCapture capture = new VideoCapture(0);
            capture.Set(CaptureProperty.FrameWidth, 1280);
            capture.Set(CaptureProperty.FrameHeight, 720);

            Mat background2 = new Mat();
            Mat finalBackground = new Mat();
            Mat finalImage = new Mat();
            Mat midImage = new Mat();
            Mat secondBackground = new Mat();
            background.CopyTo(secondBackground,mask);


            while (true)
            {
                if (!capture.Read(background2))
                {
                    continue;
                }
                background2.CopyTo(finalBackground, invertMask);
                Cv2.BitwiseOr(finalForeground, finalBackground, midImage);
                Cv2.BitwiseOr(secondBackground, midImage, finalImage);
                Cv2.ImShow("Window", finalImage);
                if(Cv2.WaitKey(1) != -1)
                {
                    break;
                }
            }



            //Cv2.ImShow("Window", finalImage);

            //Cv2.WaitKey(0);
        }
    }
}
