/*
 * Transym OCR Demonstration program
 *
 * THE SOFTWARE IS PROVIDED "AS-IS" AND WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS, IMPLIED OR OTHERWISE, INCLUDING WITHOUT LIMITATION, ANY 
 * WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE.  
 *
 * This program demonstrates calling TOCR version 5 from C#.
 * This program demonstrates the simple processing of a single file.
 * No real attempt is made to handle errors.
 *
 * Copyright (C) 2022 Transym Computer Services Ltd.
 *
 * TOCR5 DemoCSharp
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Drawing;


namespace CSharpDemo
{
    class Program : TOCRDeclares
    {
        //public const Int32 MAX_PATH = 260;
        public const string SAMPLE_TIF_FILE = "..\\..\\..\\Sample.tif";
        public const string SAMPLE_BMP_FILE = "..\\..\\..\\Sample.bmp";
        public const string SAMPLE_PDF_FILE = "..\\..\\..\\Sample.pdf";
        public const string PDF_RESULTS_FILE = "..\\..\\..\\Result.pdf";

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern UIntPtr GlobalSize(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern byte[] GlobalLock(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr GlobalUnlock(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr GlobalFree(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [Flags]
        enum FileMapProtection : uint
        {
            PageReadonly = 0x02,
            PageReadWrite = 0x04,
            PageWriteCopy = 0x08,
            PageExecuteRead = 0x20,
            PageExecuteReadWrite = 0x40,
            SectionCommit = 0x8000000,
            SectionImage = 0x1000000,
            SectionNoCache = 0x10000000,
            SectionReserve = 0x4000000,
        }

        static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr CreateFileMapping(
         IntPtr hFile,
         IntPtr lpFileMappingAttributes,
         FileMapProtection flProtect,
         uint dwMaximumSizeHigh,
         uint dwMaximumSizeLow,
         string lpName);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,
            uint dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        const UInt32 SECTION_QUERY = 0x0001;
        const UInt32 SECTION_MAP_WRITE = 0x0002;
        const UInt32 SECTION_MAP_READ = 0x0004;
        const UInt32 SECTION_MAP_EXECUTE = 0x0008;
        const UInt32 SECTION_EXTEND_SIZE = 0x0010;
        const UInt32 SECTION_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SECTION_QUERY |
        SECTION_MAP_WRITE |
        SECTION_MAP_READ |
        SECTION_MAP_EXECUTE |
        SECTION_EXTEND_SIZE);

        const UInt32 FILE_MAP_ALL_ACCESS = SECTION_ALL_ACCESS;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPFILEHEADER
        {
         public Int16 bfType;
         public Int32 bfSize;
         public Int16 bfReserved1;
         public Int16 bfReserved2;
         public Int32 bfOffBits;
        };
        
        static void Main(string[] args)
        {
            Example1();
            Example2();
            Example3();
            Example4();
            Example5();
            Example6();
            Example7();
            Example8();
            Example9();
            Example10();
        }

        // Demonstrates how to OCR a file as V5
        static void Example1()
        {
            TOCRJOBINFO_EG JobInfo_EG = new TOCRJOBINFO_EG();
            TOCRRESULTSEX_EG Results = new TOCRRESULTSEX_EG();
            Int32 Status = 0;
            Int32 JobNo = 0;
            String Msg = "";

            JobInfo_EG.Initialize();
            TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX);

            //JobInfo_EG.InputFile = SAMPLE_TIF_FILE;
            //JobInfo_EG.JobType = TOCRJOBTYPE_TIFFFILE;

            // or
            //JobInfo_EG.InputFile = SAMPLE_BMP_FILE;
            //JobInfo_EG.JobType = TOCRJOBTYPE_DIBFILE;

            // or
            JobInfo_EG.InputFile = SAMPLE_PDF_FILE;
            JobInfo_EG.JobType = TOCRJOBTYPE_PDFFILE;

            Status = TOCRInitialise(ref JobNo);

            if (Status == TOCR_OK) {
                if (OCRWait(JobNo, ref JobInfo_EG)) {
                    if (GetResults(JobNo, ref Results)) {
                        // Display the results

                        if (FormatResults(Results, ref Msg)) {
                            MessageBox.Show(Msg, "Example 1", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }

                TOCRShutdown(JobNo);
            }
        } // Example1()

        // Demonstrates how to OCR multiple files
        static void Example2()
        {
            TOCRJOBINFO_EG JobInfo_EG = new TOCRJOBINFO_EG();
            TOCRRESULTSEX_EG Results = new TOCRRESULTSEX_EG();
            Int32 Status = 0;
            Int32 JobNo = 0;
            String Msg = "";
            Int32 CountDone = 0;

            JobInfo_EG.Initialize();
            TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX);
            Status = TOCRInitialise(ref JobNo);

            if (Status == TOCR_OK) {
                 // 1st file
		        JobInfo_EG.InputFile = SAMPLE_TIF_FILE;
                JobInfo_EG.JobType = TOCRJOBTYPE_TIFFFILE;
		        if (OCRWait(JobNo, ref JobInfo_EG)) {
			        if (GetResults(JobNo, ref Results)) {
				        // Process the results
				        CountDone++;
			        }
		        }

		        // 2nd file
		        JobInfo_EG.InputFile = SAMPLE_BMP_FILE;
                JobInfo_EG.JobType = TOCRJOBTYPE_DIBFILE;
		        if (OCRWait(JobNo, ref JobInfo_EG)) {
			        if (GetResults(JobNo, ref Results)) {
				        // Process the results
				        CountDone++;
			        }
		        }

		        // 3rd file
		        JobInfo_EG.InputFile = SAMPLE_PDF_FILE;
                JobInfo_EG.JobType = TOCRJOBTYPE_PDFFILE;

                if (OCRWait(JobNo, ref JobInfo_EG)) {
			        if (GetResults(JobNo, ref Results)) {
				        // Process the results
				        CountDone++;
			        }
		        }

                TOCRShutdown(JobNo);
	        }

            Msg = CountDone.ToString();
            Msg += " of 3 jobs done";
            MessageBox.Show(Msg, "Example 2", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        } // Example2()

        // Demonstrates how to OCR an image using a memory mapped file created by TOCR
        static void Example3()
        {
            TOCRJOBINFO_EG JobInfo_EG = new TOCRJOBINFO_EG();
            TOCRRESULTSEX_EG Results = new TOCRRESULTSEX_EG();
            Int32 Status = 0;
            Int32 JobNo = 0;
            String Msg = "";
            IntPtr hMMF = new IntPtr();

            JobInfo_EG.Initialize();
            TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX);
            Status = TOCRInitialise(ref JobNo);

            if (Status == TOCR_OK) {
                Status = TOCRConvertFormat(JobNo, SAMPLE_TIF_FILE, TOCRCONVERTFORMAT_TIFFFILE, ref hMMF, TOCRCONVERTFORMAT_MMFILEHANDLE, 0);
                //Status = TOCRConvertFormat(JobNo, SAMPLE_PDF_FILE, TOCRCONVERTFORMAT_PDFFILE, ref hMMF, TOCRCONVERTFORMAT_MMFILEHANDLE, 0);

                if (Status == TOCR_OK) {
                    SafeFileHandle hSMMF = new SafeFileHandle(hMMF, true);

                    JobInfo_EG.JobType = TOCRJOBTYPE_MMFILEHANDLE;
                    JobInfo_EG.hMMF = hMMF;

                    if (OCRWait(JobNo, ref JobInfo_EG)) {
				        if ( GetResults(JobNo, ref Results) ) {
					        FormatResults(Results, ref Msg);
                            MessageBox.Show(Msg, "Example 3", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				        }
			        }
                    // as hSMMF goes out of scope it will close hMMF
                }
                TOCRShutdown(JobNo);
	        }
        } // Example3()

        // Demonstrates how to OCR an image using a memory mapped file created here
        static void Example4()
        {
            TOCRJOBINFO_EG JobInfo_EG = new TOCRJOBINFO_EG();
            TOCRRESULTSEX_EG Results = new TOCRRESULTSEX_EG();
            Int32 Status = 0;
            Int32 JobNo = 0;
            String Msg = "";
            SafeMemoryMappedFileHandle hSMMF;
            Int32 numbytes = 0;
	        BITMAPFILEHEADER	bf;

            JobInfo_EG.Initialize();

	        // Create a memory mapped file from a DIB file
	        // The contents of a memory mapped file for TOCR are everything in the bitmap file except
	        // the bitmap file header.
            FileStream fs = new FileStream(@SAMPLE_BMP_FILE, FileMode.Open, FileAccess.Read);

	        // read the bitmap file header to find the size of the bitmap
            byte[] buffer = new byte[Marshal.SizeOf(typeof(BITMAPFILEHEADER))];
            fs.Read(buffer, 0, Marshal.SizeOf(typeof(BITMAPFILEHEADER)));
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            bf = (BITMAPFILEHEADER)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(BITMAPFILEHEADER));
            handle.Free();

            // place just the bitmap (excluding the file header) into the MMF
			numbytes = bf.bfSize - Marshal.SizeOf(typeof(BITMAPFILEHEADER));
            MemoryMappedFile PagedMemoryMapped = MemoryMappedFile.CreateOrOpen("any unique name", numbytes, MemoryMappedFileAccess.ReadWrite);
            MemoryMappedViewStream FileMap = PagedMemoryMapped.CreateViewStream();
            fs.CopyTo(FileMap); // read the rest of the bitmap into the memory mapped file

            // set up the job structure
            JobInfo_EG.JobType = TOCRJOBTYPE_MMFILEHANDLE;
            hSMMF = PagedMemoryMapped.SafeMemoryMappedFileHandle;
            JobInfo_EG.hMMF = hSMMF.DangerousGetHandle();

            TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX);

	        Status = TOCRInitialise(ref JobNo);

	        if ( Status == TOCR_OK ) {
                if (OCRWait(JobNo, ref JobInfo_EG)) {
				    if ( GetResults(JobNo, ref Results) ) {
					    if(FormatResults(Results, ref Msg)) {
                            MessageBox.Show(Msg, "Example 4", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					    } else {
                            MessageBox.Show("", "Example 4 - no results found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					    }
			        }
		        }
		        TOCRShutdown(JobNo);
	        }
            // as hSMMF goes out of scope it will close hMMF
        } // Example4()

        // Retrieve information on Job Slot usage
        static void Example5()
        {
            Int32 Status = 0;
            Int32[] JobSlotInf;
            GCHandle JobSlotInfGC; //  handle to pinned JobSlotInf[]
            String Msg = "Slot usage is\n";
	        Int32				NumSlots;

            TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX);

	        // uncomment to see effect on usage
            //Int32 JobNo = 0;
            //Status = TOCRInitialise(ref JobNo);

	        NumSlots = TOCRGetJobDBInfo(IntPtr.Zero);

            if (NumSlots > 0) {
                JobSlotInf = new Int32[NumSlots];
                JobSlotInfGC = GCHandle.Alloc(JobSlotInf, GCHandleType.Pinned);
                IntPtr AddOfJobSlotInfGC = JobSlotInfGC.AddrOfPinnedObject();
                Status = TOCRGetJobDBInfo(AddOfJobSlotInfGC);

                if (Status == TOCR_OK) {
                    for (Int32 SlotNo = 0; SlotNo < NumSlots; SlotNo++) {
                        Msg += "\nSlot ";
                        Msg += SlotNo;
                        Msg += " is ";

                        switch (JobSlotInf[SlotNo]) {
                            case TOCRJOBSLOT_FREE:
                                Msg += "free";
                                break;
                            case TOCRJOBSLOT_OWNEDBYYOU:
                                Msg += "owned by you";
                                break;
                            case TOCRJOBSLOT_BLOCKEDBYYOU:
                                Msg += "blocked by you";
                                break;
                            case TOCRJOBSLOT_OWNEDBYOTHER:
                                Msg += "owned by another process";
                                break;
                            case TOCRJOBSLOT_BLOCKEDBYOTHER:
                                Msg += "blocked by another process";
                                break;
                        }
                    }
                    MessageBox.Show(Msg, "Example 5", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                JobSlotInfGC.Free();
            } else {
                MessageBox.Show("No job slots", "Example 5", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

	        //TOCRShutdown(JobNo);
        } // Example5()

        // Retrieve information on Job Slots
        static void Example6()
        {
            Int32 Status = 0;
            Int32 JobNo = 0;
            String Msg = "Slot usage is\n";
	        Int32		NumSlots;
	        byte[]		Licence;
            GCHandle    LicenceGC; //  handle to pinned Licence[]
            Int32 Volume = 0;
            Int32 Time = 0;
            Int32 Remaining = 0;
            Int32 Features = 0;

            TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX);

	        // comment to see effect on usage
	        Status = TOCRInitialise(ref JobNo);

	        NumSlots = TOCRGetJobDBInfo(IntPtr.Zero);
	        if ( NumSlots > 0 ) {
		        for (Int32 SlotNo = 0; SlotNo < NumSlots; SlotNo++) {
			        Msg += "\nSlot ";
                    Msg += SlotNo;
                    Msg += " is ";

                    Licence = new byte[20];
                    LicenceGC = GCHandle.Alloc(Licence, GCHandleType.Pinned);
                    IntPtr AddOfLicenceGC = LicenceGC.AddrOfPinnedObject();

                    Status = TOCRGetLicenceInfoEx(SlotNo, AddOfLicenceGC, ref Volume, ref Time, ref Remaining, ref Features);
			        if ( Status == TOCR_OK ) {
                        String LicenceStr = "";
                        char c = ((char)Licence[0]);

                        for (Int32  i = 1; (i < 20) && (c > 0); i++) {
                            LicenceStr += c;
                            c = ((char)Licence[i]);
                        }

                        Msg += LicenceStr;

				        switch (Features)
				        {
					        case TOCRLICENCE_STANDARD:
						        Msg += " STANDARD licence";
						        break;
					        case TOCRLICENCE_EURO:
						        if ( LicenceStr == "154C-43BA-9421-C925" )
							        Msg += " EURO TRIAL licence";
						        else 
							        Msg += " EURO licence";
						        break;
					        case TOCRLICENCE_EUROUPGRADE:
						        Msg += " EURO UPGRADE licence";
						        break;
					        case TOCRLICENCE_V3SE:
						        if ( LicenceStr == "2E72-2B35-643A-0851" )
							        Msg += " V3 SE TRIAL licence";
						        else 
							        Msg += " V3 licence";
						        break;
					        case TOCRLICENCE_V3SEUPGRADE:
						        Msg += " V1/2 UPGRADE to V3 SE licence";
						        break;
					        case TOCRLICENCE_V3PRO:
						        Msg += " V3 Pro/V4/V5 licence";
						        break;
					        case TOCRLICENCE_V3PROUPGRADE:
						        Msg += " V1/2 UPGRADE to V3 Pro/V4/V5 licence";
						        break;
					        case TOCRLICENCE_V3SEPROUPGRADE:
						        Msg += " V3 SE UPGRADE to V3 Pro/V4/V5 licence";
						        break;
				        }
				        if ( Volume != 0 || Time != 0 ) {
                             Msg += " ";
					         Msg += Remaining;

                            if ( Time != 0 ) {
						        Msg += " days";
                            } else {
						        Msg += " A4 pages";
                            }
					        Msg += " remaining on licence";
				        }
			        } else
				        Msg += "Failed to get information";	
		        }
		        MessageBox.Show(Msg, "Example 6", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
	        } else 
		        MessageBox.Show("No job slots", "Example 6", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

	        TOCRShutdown(JobNo);
        } // Example6()
        
        // Get images from a TWAIN compatible device
        static void Example7()
        {
            TOCRJOBINFO_EG JobInfo_EG = new TOCRJOBINFO_EG();
            TOCRRESULTSEX_EG Results = new TOCRRESULTSEX_EG();
            Int32 Status = 0;
            Int32 JobNo = 0;
            String Msg = "";
	        Int32		NumImages = 0;
	        Int32		CntImages = 0;
	        IntPtr[]	hMems;
            GCHandle    hMemsGC; //  handle to pinned hMems[]

            JobInfo_EG.Initialize();
	        TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX);

	        Status = TOCRInitialise(ref JobNo);

	        Status = TOCRTWAINSelectDS(); // select the TWAIN device
	        if ( Status == TOCR_OK) {
		        Status = TOCRTWAINShowUI(1);
		        Status = TOCRTWAINAcquire(ref NumImages);
		        if ( NumImages > 0 ) {
			        hMems = new IntPtr[NumImages];
                    hMemsGC = GCHandle.Alloc(hMems, GCHandleType.Pinned);
                    IntPtr AddOfhMemsGC = hMemsGC.AddrOfPinnedObject();

				    Status = TOCRTWAINGetImages(AddOfhMemsGC);
				    for (Int32 ImgNo = 0; ImgNo < NumImages; ImgNo++) {
					    if ( hMems[ImgNo] != IntPtr.Zero ) {
						    // convert the memory block to a Memory Mapped File
                            SafeMemoryMappedFileHandle hSMMF = ConvertGlobalMemoryToMMF(hMems[ImgNo]);
						    // free the global memory block to save space
						    GlobalFree(hMems[ImgNo]);

						    if ( (!hSMMF.IsClosed) && (!hSMMF.IsInvalid) ) {
							    JobInfo_EG.JobType = TOCRJOBTYPE_MMFILEHANDLE;
							    JobInfo_EG.hMMF = hSMMF.DangerousGetHandle();
							    if ( OCRWait(JobNo, ref JobInfo_EG) ) {
								    if ( GetResults(JobNo, ref Results) ) {
									    if ( FormatResults(Results, ref Msg) ) {
										    MessageBox.Show(Msg, "Example 7", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									    }
								    }
							    }

							    CntImages ++;
							    // as hSMMF goes out of scope it will close hMMF
						    }

					    }
				    }
		        }
	        }
	        TOCRShutdown(JobNo);

            Msg = CntImages.ToString();
	        Msg += " images successfully acquired\n";
	        MessageBox.Show(Msg, "Example 7", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        } // Example7()


        // Demonstrates TOCRSetConfig and TOCRGetConfig
        static void Example8()
        {
            Int32   Status = 0;
            Int32   JobNo = 0;
            String  Msg = "";
	        Int32	Value = 0;
	        StringBuilder Line = new StringBuilder();

	        // Override the INI file settings for all new jobs
	        TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX);
	        TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_SRV_ERRORMODE, TOCRERRORMODE_MSGBOX);

	        TOCRGetConfigStr(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_LOGFILE, Line);
	        Msg = "Default Log file name ";
            Msg += Line;
	        MessageBox.Show(Msg, "Example 8", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

	        TOCRSetConfigStr(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_LOGFILE, "Loggederrs.lis");
	        TOCRGetConfigStr(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_LOGFILE, Line);
	        Msg = "New default Log file name ";
            Msg += Line;
	        MessageBox.Show(Msg, "Example 8", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

	        Status = TOCRInitialise(ref JobNo);
	        TOCRSetConfig(JobNo, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_NONE);

	        TOCRGetConfig(JobNo, TOCRCONFIG_DLL_ERRORMODE, ref Value);
	        Msg = "Job ";
            Msg += JobNo;
            Msg += " DLL error mode ";
            Msg += Value;
	        MessageBox.Show(Msg, "Example 8", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

	        TOCRGetConfig(JobNo, TOCRCONFIG_SRV_ERRORMODE, ref Value);
	        Msg = "Job ";
            Msg += JobNo;
            Msg += " Service error mode ";
            Msg += Value;
	        MessageBox.Show(Msg, "Example 8", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

	        // Cause an error - then check Loggederrs.lis
	        TOCRSetConfig(JobNo, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_LOG);
	        TOCRSetConfig(JobNo, 1000, TOCRERRORMODE_LOG);

	        TOCRShutdown(JobNo);

        } // Example8()

        // Demonstrates how to OCR a file and place the results in a PDF document
        static void Example9()
        {
	        TOCRPROCESSPDFOPTIONS_EG PDFOpts = new TOCRPROCESSPDFOPTIONS_EG();
            TOCRJOBINFO_EG JobInfo_EG = new TOCRJOBINFO_EG();
            TOCRRESULTSEX_EG Results = new TOCRRESULTSEX_EG();
            Int32 Status = 0;
            Int32 JobNo = 0;
            String Msg = "";

            JobInfo_EG.Initialize();
            TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX);
	        //TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_LOG); // TODO: NP - just testing; use above line normally
	        
            // set the PDF options
	        PDFOpts.ResultsOn = TOCRPDFDeclares.VARIANT_TRUE;
	        PDFOpts.OriginalImageOn = TOCRPDFDeclares.VARIANT_TRUE;
	        PDFOpts.ProcessedImageOn = TOCRPDFDeclares.VARIANT_TRUE;

	        // set the job options as Example1()
	        JobInfo_EG.InputFile = SAMPLE_TIF_FILE;
	        JobInfo_EG.JobType = TOCRJOBTYPE_TIFFFILE;

	        // or
	        //JobInfo_EG.InputFile = SAMPLE_BMP_FILE;
	        //JobInfo_EG.JobType = TOCRJOBTYPE_DIBFILE;

	        // or
	        //JobInfo_EG.InputFile = SAMPLE_PDF_FILE;
	        //JobInfo_EG.JobType = TOCRJOBTYPE_PDFFILE;

	        JobInfo_EG.ProcessOptions.CGAlgorithm = TOCRJOBCG_HISTOGRAM;

	        Status = TOCRInitialise(ref JobNo);

	        if ( Status == TOCR_OK ) {
		        // Use the PDF version of the OCR process
		        if ( OCRWait_PDF(JobNo, ref JobInfo_EG, PDF_RESULTS_FILE, ref PDFOpts) ) {
			        if ( GetResults(JobNo, ref Results) ) {
						// Display the results
						FormatResults(Results, ref Msg);
						MessageBox.Show(Msg, "Example 9 - V5 PDF test", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			        }
		        }

		        TOCRShutdown(JobNo);
	        }
        } // Example9()

        // Example10()
        // 2) create a block of memory for the PDFExtractor to write to.
        // 3) Allocate a block of memory to store the metadata
        // 4) create a memory based pdf document for the extractor to work with called hDocIn.
        // 5) create a memory based pdf document for archiver to write to called docOut.
        // 7) Use the pdf extractor to load the file into the ExtractorMemDoc called hDocIn
        // 8) Load the original document into the archiver memory
        // 10) Connect to TOCRService
        // 11) remove the text from each page and save an image of the rest of the page as a DIB
        // 12) Extract the number of pages from the PDF
        // 13) Create a memory mapped file i.e. reserve a large enough memory file space.
        // 14) Add the file name to metadata
        // 15) Record in the metadata that the following pages will be the original document.
        // 16) Extract the entire input pdf and save it to docOut
        // 17) Record in the metadata that the following pages will be the text returned by the OCR
        // 18) Loop through all pages in the input file.
	        // 18a) Initialise the Page store to only contain the page to be analysed
	        // 18b) find the page size in inches
	        // 18c) populate dpi values if known
	        // 18d) Allocate memory for the dib file mmf process (i.e. reserve it from our memory mapped file created earlier)
	        // 18e) Extract the mmf view (i.e. the memory allocated for the dib file mmf process) and interpret it as the extractors' memory block.
	        // 18f) Create a DIB file from the page stored in memory, using the reccomended values storing the result in an mmf view
	        // 18g) clear and return the memory used in the MMF process by the DIB
	        // 18h) if the extractor has found something of OCR relevence on the page.
		        // 18i) record for TOCR that we will be sending an MMF job
		        // 18j) record for TOCR the address of the memory mapped file
		        // 18l) record in the metadata the original page number 
		        // 18m) save the OCR results and metadata to a new PDF page at the end of the document in the archiver's memory
        // 19) move on a page
        // 20) If all has gone well, save the memory pdf document out to a file
        static void Example10()
        {
            TOCRJOBINFO_EG JobInfo_EG = new TOCRJOBINFO_EG();
            TOCRRESULTSEX_EG Results = new TOCRRESULTSEX_EG();
            Int32 Status = 0;
            Int32 JobNo = 0;
            String Msg = "";
	        UInt32				NumImages = 0;
	        Int32				CntImages = 0;
	        TOCRPDFDeclares.TOCRPDF_COLOUR_MODE ColourMode = TOCRPDFDeclares.TOCRPDF_COLOUR_MODE.COLOUR_MODE_GREY8;  // do all processing as RGB (false => Grey8)
	        String ResultChars = "";

            JobInfo_EG.Initialize();

	        // a4 page at 300 dpi; x = 2479, y = 3508; x * y = 8697159 pixels
	        // for RGB - times pixels by 3 bytes = 26091477 (26MB)
	        const UInt32 MaxSizeLow = 30000000;  // a4 RGB page at 300 dpi, no compression

	        //String ErrorStore = "";
	        // 2) create a block of memory for the PDFExtractor to write to.
	        TOCRPDFDeclares.CHARPTR_WITH_LEN cpwl;
            
	        // 3) Allocate a block of memory to store the metadata
            TOCRPDFDeclares.TocrResultsInfo ResultsInfo = new TOCRPDFDeclares.TocrResultsInfo();
            ResultsInfo.Initialize();

            // extractor creation
            TOCRPDFDeclares.PDFExtractor extractor = new TOCRPDFDeclares.PDFExtractor();
	        TOCRPDFDeclares.PDFExtractorPage page = new TOCRPDFDeclares.PDFExtractorPage(extractor);
            // 4) create a memory based pdf document for the extractor to work with called hDocIn.	
            TOCRPDFDeclares.PDFExtractorMemDoc docIn = new TOCRPDFDeclares.PDFExtractorMemDoc(extractor);
	        
            // archiver creation
            TOCRPDFDeclares.PDFArchiver	archiver = new TOCRPDFDeclares.PDFArchiver();
            TOCRPDFDeclares.PDFArchiverMemDoc docAIn = new TOCRPDFDeclares.PDFArchiverMemDoc(archiver);
            
            // 5) create a memory based pdf document for archiver to write to called docOut.
            TOCRPDFDeclares.PDFArchiverMemDoc docOut = new TOCRPDFDeclares.PDFArchiverMemDoc(archiver);
	
            // 7) Use the pdf extractor to load the file (name held in SAMPLE_PDF_FILE)  into the ExtractorMemDoc hDocIn
            docIn.Load(SAMPLE_PDF_FILE);
            
            // 8) Load the original document into the archiver memory
            docAIn.Load(SAMPLE_PDF_FILE);

	        TOCRSetConfig(TOCRCONFIG_DEFAULTJOB, TOCRCONFIG_DLL_ERRORMODE, TOCRERRORMODE_MSGBOX);
	        // 10) Connect to TOCRService
	        Status = TOCRInitialise(ref JobNo);
	        
	        // 11) remove the text from each page and save an image of the rest of the page as a DIB
	        // 12) Extract the number of pages from the PDF
            NumImages = docIn.GetPageCount();
	        
	        // 13) Create a memory mapped file i.e. reserve a large enough memory file space.
	        IntPtr hMMF = CreateFileMapping(INVALID_HANDLE_VALUE, IntPtr.Zero, FileMapProtection.PageReadWrite, 0, MaxSizeLow, "");
            //MemoryMappedFile PagedMemoryMapped = MemoryMappedFile.CreateOrOpen("any unique name", MaxSizeLow, MemoryMappedFileAccess.ReadWrite);
            //SafeMemoryMappedFileHandle hSMMF = PagedMemoryMapped.SafeMemoryMappedFileHandle;
            //Record that we intend to include original document in results
            ResultsInfo.bContainedInThisDocument = TOCRPDFDeclares.VARIANT_TRUE;
            // 14) Add the file name to metadata
            Int32 Len = Math.Min(TOCRPDFDeclares._MAX_PATH, SAMPLE_PDF_FILE.Length);
            SAMPLE_PDF_FILE.CopyTo(0, ResultsInfo.OriginalFileName, 0, Len);
     
	        // 15) Record in the metadata that the following pages will be the original document.
            ResultsInfo.OutputStage = TOCRPDFDeclares.TocrResultStage.TRS_Original_Page;
	        // 16) Extract the entire input pdf and save it to docOut, sets the Input file(docAIn) to be inserted at the end of the docOut
            docOut.SaveAllPdf(docAIn, -2, ResultsInfo);
	        // 17) Record in the metadata that the following pages will be the text returned by the OCR
            ResultsInfo.OutputStage = TOCRPDFDeclares.TocrResultStage.TRS_Text;
            
	        // 18) Loop through all pages in the input file.
            for (UInt32 ImgNo = 0; ImgNo < NumImages; ImgNo++)
            {
                // 18a) Initialise the Page store to only contain the page to be analysed
                page = docIn.GetPage(ImgNo + 1);  // get page is 1 based but ImgNo is zero based
                page.m_ColourMode = ColourMode;

                // 18b) find the page size in inches
                page.m_size = page.FindPageSize();
                page.m_size.width = (page.m_size.width < 0) ? (-1.0 * page.m_size.width) : page.m_size.width;
                page.m_size.height = (page.m_size.height < 0) ? (-1.0 * page.m_size.height) : page.m_size.height;
                // 18c) populate dpi values if known (0 or less if not known)
                page.m_DPI = page.GetRecommendedDPI(page.m_size, page.m_ColourMode);

                 { // scope to define the life of the memory mapped view
                    // 18d) Allocate memory for the dib file mmf process (i.e. reserve it from our memory mapped file created earlier)
                    //MemoryMappedViewAccessor View = PagedMemoryMapped.CreateViewAccessor();
                    //IntPtr View = MapViewOfFile(hSMMF.DangerousGetHandle(), (int)FILE_MAP_ALL_ACCESS, 0, 0, 0);
                    IntPtr View = MapViewOfFile(hMMF, FILE_MAP_ALL_ACCESS, 0, 0, 0);

                    // 18e) Extract the mmf view (i.e. the memory allocated for the dib file mmf process) and interpret it as the extractors' memory block.
                    //SafeMemoryMappedViewHandle hSMMV = View.SafeMemoryMappedViewHandle;
                    //byte* ptr = (byte*)0;
                    //hSMMV.AcquirePointer(ref ptr);
                    cpwl.charPtr = View;
                    cpwl.len = MaxSizeLow;

                    // 18f) Create a DIB file from the page stored in memory, using the recommended values storing the result in an mmf view (cpwl)
                    page.PageToDib(cpwl);
                    //A page may be processed as blank (even when there appears to be something there) 
                    // because there is nothing on the page that is relevent to the OCR process.
                    // 18g) clear and return the memory used in the MMF process by the DIB
                    cpwl.len = 0;
                    //cpwl.charPtr = IntPtr.Zero;
                    // as View goes out of scope it will close and delete the memory mapped view.
                }

                // 18h) if the extractor has found something of OCR relevance on the page.
                if (page.m_PageIsNotBlank)
                {
                    // 18i) record for TOCR that we will be sending an MMF job
                    JobInfo_EG.JobType = TOCRJOBTYPE_MMFILEHANDLE;
                    // 18j) record for TOCR the address of the memory mapped file
                    //SafeMemoryMappedFileHandle hSMMF = PagedMemoryMapped.SafeMemoryMappedFileHandle;
                    //JobInfo_EG.hMMF = hSMMF.DangerousGetHandle();
                    JobInfo_EG.hMMF = hMMF;
                    //JobInfo2.ProcessOptions.StructId = 1; //XPos and YPos refer to the positions on the original page 
                    if (OCRWait(JobNo, ref JobInfo_EG))
                    {
                        if (GetResults(JobNo, ref Results))
                        {
                            #if DEBUG
                                /* useful for debug
						        String Msg = "";
						        if ( FormatResults(Results, Msg) ) {
						        FormatResults(Results, Msg);
						        MessageBox(NULL, Msg, "Example 10", MB_TASKMODAL | MB_TOPMOST);
						        }
						        */
                            #endif // _DEBUG
                            if (Results.Hdr.NumItems > 0)
                            {
                                // Display the results
                                FormatResults(Results, ref ResultChars);

                                // 18l) record in the metadata the original page number 
                                ResultsInfo.OriginalPageNumber = ImgNo;
                                // 18m) save the OCR results and metadata to a new PDF page at the end of the document in the archiver's memory
                                docOut.SaveAppendix(ResultChars, -2, ResultsInfo);
                            } // if ( Results->Hdr.NumItems > 0 )
                            // 19) move on a page
                            CntImages++;
                        } // if ( GetResults(JobNo, &Results) )
                    } // if ( OCRWait(JobNo, JobInfo2) )
                } // if(pageIsNotBlank)  // may have been set by the rendering engine
            } // for(ImgNo

	        // 20) If all has gone well, save the memory pdf document out to a file
            docOut.Close(PDF_RESULTS_FILE);
            
		    // notify the user
            Msg = "Success. Please see ";
            Msg += PDF_RESULTS_FILE;
            MessageBox.Show(Msg, "Example 10", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        } // Example10

        //  Function to retrieve the results from the service process and load into 'ResultsEx_EG'
        static bool GetResults(int JobNo, ref TOCRRESULTSEX_EG ResultsEx_EG)
        {
            Int32 ResultsInf = 0; //  number of bytes needed for results
            IntPtr AddrOfItemBytes;
            Int32 ItemNo; //  loop counter
            Int32 AltNo;  //  loop counter
            byte[] Bytes; //  array of bytes of returned results
            GCHandle BytesGC; //  handle to pinned Bytes[]
            Int32 Offset; //  address offset into Bytes[]
            TOCRRESULTSITEMEXHDR_EG ItemHdr;
            bool result = false;
            ResultsEx_EG.Hdr.NumItems = 0;

            if ((TOCRGetJobResultsEx_EG(JobNo, TOCRGETRESULTS_EXTENDED_EG, ref ResultsInf, IntPtr.Zero) == TOCR_OK)) {
                if ((ResultsInf > 0)) {
                    Bytes = new byte[ResultsInf];
                    BytesGC = GCHandle.Alloc(Bytes, GCHandleType.Pinned);
                    IntPtr AddOfBytesGC = BytesGC.AddrOfPinnedObject();
                    
                    if ((TOCRGetJobResultsEx_EG(JobNo, TOCRGETRESULTS_EXTENDED_EG, ref ResultsInf, AddOfBytesGC) == TOCR_OK)) {
                        ResultsEx_EG.Hdr = (TOCRRESULTSHEADER_EG)Marshal.PtrToStructure(AddOfBytesGC, typeof(TOCRRESULTSHEADER_EG));
                        
                        if (ResultsEx_EG.Hdr.NumItems > 0) {
                            ResultsEx_EG.Item = new TOCRRESULTSITEMEX_EG[ResultsEx_EG.Hdr.NumItems];
                            Offset = Marshal.SizeOf(typeof(TOCRRESULTSHEADER_EG));
                            
                            for (ItemNo = 0; (ItemNo < ResultsEx_EG.Hdr.NumItems); ItemNo++) {
                                AddrOfItemBytes = Marshal.UnsafeAddrOfPinnedArrayElement(Bytes, Offset);
                                //  Cannot Marshal TOCRRESULTSITEMEX so use copy of structure header
                                //  This unfortunately means a double copy of the data
                                ItemHdr = ((TOCRRESULTSITEMEXHDR_EG)(Marshal.PtrToStructure(AddrOfItemBytes, typeof(TOCRRESULTSITEMEXHDR_EG))));
                                TOCRRESULTSITEMEX_EG element = new TOCRRESULTSITEMEX_EG();
                                element.Initialize(ItemHdr);
                                ResultsEx_EG.Item[ItemNo] = element;
                                Offset = (Offset + Marshal.SizeOf(typeof(TOCRRESULTSITEMEXHDR_EG)));

                                for (AltNo = 0; (AltNo <= 4); AltNo++) {
                                    AddrOfItemBytes = Marshal.UnsafeAddrOfPinnedArrayElement(Bytes, Offset);
                                    ResultsEx_EG.Item[ItemNo].Alt[AltNo] = ((TOCRRESULTSITEMEXALT_EG)(Marshal.PtrToStructure(AddrOfItemBytes, typeof(TOCRRESULTSITEMEXALT_EG))));
                                    Offset = (Offset + Marshal.SizeOf(typeof(TOCRRESULTSITEMEXALT_EG)));
                                } // for AltNo
                            } // for ItemNo                   

                            result = true;
                        } // ResultsEx_EG.Hdr.NumItems > 0
                    } //  TOCRGetJobResults_EG(JobNo, ResultsInf, Bytes) = TOCR_OK
                    BytesGC.Free();
                } // ResultsInf > 0
            } //  TOCRGetJobResults_EG(JobNo, ResultsInf, 0) = TOCR_OK

            return result;
        }

        static bool OCRWait(Int32 JobNo, ref TOCRJOBINFO_EG JobInfo_EG)
        {
            Int32 Status = 0;
            Int32 JobStatus = 0;
            bool result = false;

            Status = TOCRDoJob_EG(JobNo, ref JobInfo_EG);

            if (Status == TOCR_OK) {
                Status = TOCRGetJobStatus(JobNo, ref JobStatus);

                while ((Status == TOCR_OK) && (JobStatus == TOCRJOBSTATUS_BUSY)) {
                    //  Just Doevents absorbs too much CPU whilst Sleep
                    //  alone will lock the application - so this is a compromise
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    Application.DoEvents();

                    // check again
                    Status = TOCRGetJobStatus(JobNo, ref JobStatus);
                }
            }

            if ((Status == TOCR_OK) && (JobStatus == TOCRJOBSTATUS_DONE)) {
                result = true;
            }

            return result;
        }

        static bool FormatResults(TOCRRESULTSEX_EG Results, ref String FormattedResults)
        {
            Int32 ItemNo;
	        bool Status = false;
            char C1; //  a character
            
            FormattedResults = "";

	        if ( Results.Hdr.NumItems > 0 ) {
		        for (ItemNo = 0; ItemNo < Results.Hdr.NumItems; ItemNo ++ ) {
                    C1 = ((char)(Results.Item[ItemNo].OCRCharWUnicode));

                    if (C1 == '\r')
                    {
                        FormattedResults += '\n';
                    } else {
                        FormattedResults += C1;
                    }
		        }
		        Status = true;
	        }

	        return Status;
        }

        static SafeMemoryMappedFileHandle ConvertGlobalMemoryToMMF(IntPtr hMem)
        {
            IntPtr View;
	        //HANDLE				hMMF = NULL;
	        byte[]				ucImg;
	        //long				Status;
            SafeMemoryMappedFileHandle hSMMF;

	        //Status = TOCR_OK + 1;
            Int32 numbytes = (Int32)GlobalSize(hMem);
	        //hMMF = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, (DWORD)numbytes, 0);
            MemoryMappedFile PagedMemoryMapped = MemoryMappedFile.CreateNew("", numbytes, MemoryMappedFileAccess.ReadWrite);
            hSMMF = PagedMemoryMapped.SafeMemoryMappedFileHandle;

	        if ( (!hSMMF.IsClosed) && (!hSMMF.IsInvalid) ) {
		        View = MapViewOfFile(hSMMF.DangerousGetHandle(), FILE_MAP_ALL_ACCESS, 0, 0, 0);
                //MemoryMappedViewAccessor ViewAccessor = PagedMemoryMapped.CreateViewAccessor();

		        if ( View != IntPtr.Zero ) {
                    //SafeMemoryMappedViewHandle hSMMV = ViewAccessor.SafeMemoryMappedViewHandle;
                    //MemoryMappedViewStream FileMap = PagedMemoryMapped.CreateViewStream();

			        ucImg = GlobalLock(hMem);
			        //if ( ucImg ) {
				        //memcpy(View, ucImg, GlobalSize((void *)hMem) * sizeof(unsigned char));
                    try
                    {
                        //byte* ptr = (byte*)0;
                        //hSMMV.AcquirePointer(ref ptr);

                        Marshal.Copy(ucImg, 0, View, numbytes);
                        //Marshal.Copy(ucImg, 0, IntPtr.Add(new IntPtr(ptr), 0), numbytes);
                        //hSMMV.ReleasePointer();
                        //Status = TOCR_OK;
                    }
                    finally
                    {
                        GlobalUnlock(hMem);
                    }
			        //}
			        UnmapViewOfFile(View);
		        }
	        }

	        return hSMMF;
        } // ConvertGlbalMemoryToMMF()

        static bool OCRWait_PDF(Int32 JobNo, ref TOCRJOBINFO_EG JobInfoEg, String filename, ref TOCRPROCESSPDFOPTIONS_EG PdfOpts)
        {
            Int32   Status = TOCR_OK;
            Int32   JobStatus = TOCRJOBSTATUS_ERROR;
            Int32   ErrorMode = TOCRERRORMODE_NONE;
	        StringBuilder	Msg = new StringBuilder();

	        Status = TOCRDoJobPDF_EG(JobNo, ref JobInfoEg, filename, ref PdfOpts);
	        if (Status == TOCR_OK) {
		        Status = TOCRWaitForJob(JobNo, ref JobStatus);
	        }
	
	        if (Status == TOCR_OK && JobStatus == TOCRJOBSTATUS_DONE)
	        {
		        return true;
	        } else {
		        // If something has gone wrong display a message
		        // (Check that the OCR engine hasn't already displayed a message)
		        TOCRGetConfig(JobNo, TOCRCONFIG_DLL_ERRORMODE, ref ErrorMode);
		        if ( ErrorMode == TOCRERRORMODE_NONE ) {
			        TOCRGetJobStatusMsg(JobNo, Msg);
                    MessageBox.Show(Msg.ToString(), "OCRWait_PDF", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		        }
		        return false;
	        }
        } // OCRWait_PDF()
    } // class Program
} // namespace CSharpDemo
