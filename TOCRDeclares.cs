// ***************************************************************************
//  Module:     TOCRDeclares
// 
//  TOCR declares Version 5.1.0.0

using System.Runtime.InteropServices;
class TOCRDeclares {
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRPROCESSPDFOPTIONS_EG {

        public byte ResultsOn;        // V5 addition
        public byte OriginalImageOn;  // V5 addition
        public byte ProcessedImageOn; // V5 addition
        public System.Int32 PDFSpare;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRCHAROPTIONS_EG {
        
        // Line below: V5 extended from 601 to 608
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=608)]
        public byte[] DisableCharW;

        public void Initialize()
        {
            DisableCharW = new byte[608];
        }
    }
    
    // TOCRCHAROPTIONS_EG
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRLANGUAGEOPTIONS_EG {
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=46)]
        public byte[] DisableLangs;
        
        public void Initialize() {
            DisableLangs = new byte[46];
        }
    }
    
    // TOCRLANGUAGEOPTIONS_EG
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRPROCESSOPTIONS_EG {
        public System.Int32 StructId;
        //  4 bytes

        public byte InvertWholePage;
        public byte DeskewOff;
        public byte Orientation;
        public byte NoiseRemoveOff;
        //  8 bytes

        public byte ReturnNoiseOn;
        // v5 addition
        public byte LineRemoveOff;
        public byte DeshadeOff;
        public byte InvertOff;
        //  12 bytes

        public byte SectioningOn;
        public byte MergeBreakOff;
        public byte LineRejectOff;
        public byte CharacterRejectOff;
        //  16 bytes

        public byte ResultsReference;
        public byte LexMode;
        public byte OCRBOnly;
        public byte Speed;
        //  20 bytes

        public byte FontStyleInfoOff;
        public byte Reserved1;
        public byte Reserved2;
        public byte Reserved3;
        //  24 bytes

        public System.Int32 CCAlgorithm;
        //  28 bytes

        public float CCThreshold;
        //  32 bytes

        public System.Int32 CGAlgorithm;
        // V5 addition
        //  36 bytes

        public System.Int32 ExtraInfFlags;
        //  40 bytes

        [MarshalAs(UnmanagedType.ByValArray, SizeConst=46)]
        public byte[] DisableLangs;
        public byte Reserved4;
        public byte Reserved5;
        //  88 bytes

        // Line below: V5 extended from 601 to 608
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=608)]
        public byte[] DisableCharW;
        
        //  696 bytes
        public void Initialize()
        {
            DisableLangs = new byte[46];
            DisableCharW = new byte[608];
        }
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
    public struct TOCRJOBINFO_EG
    {
        public System.IntPtr hMMF;
        public string InputFile;
        public System.Int32 StructId;
        public System.Int32 JobType;
        public System.Int32 PageNo;
        public TOCRPROCESSOPTIONS_EG ProcessOptions;
    
        public void Initialize()
        {
            hMMF = default(System.IntPtr);
            InputFile = "";
            StructId = default(System.Int32);
            JobType = default(System.Int32);
            PageNo = default(System.Int32);
            ProcessOptions = new TOCRPROCESSOPTIONS_EG();
            ProcessOptions.Initialize();
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSHEADER_EG {
        public System.Int32 StructId;
        public System.Int32 XPixelsPerInch;
        public System.Int32 YPixelsPerInch;
        public System.Int32 NumItems;
        public float MeanConfidence;
        public System.Int32 DominantLanguage;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSITEM_EG {
        public float Confidence;
        public System.UInt16 StructId;
        public System.UInt16 OCRCharWUnicode;        // V5 split from OCRChaW
        public System.UInt16 OCRCharWInternal;        // V5 split from OCRChaW
        public System.UInt16 FontID;
        public System.UInt16 FontStyleInfo;
        public System.UInt16 XPos;
        public System.UInt16 YPos;
        public System.UInt16 XDim;
        public System.UInt16 YDim;
        public System.UInt16 YDimRef;
        public System.UInt16 Noise;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTS_EG {
        public TOCRRESULTSHEADER_EG Hdr;
        public TOCRRESULTSITEM_EG[] Item;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSITEMEXALT_EG {
        public float Factor;
        public System.UInt16 Valid;
        public System.UInt16 OCRCharWUnicode;        // V5 split from OCRChaW
        public System.UInt16 OCRCharWInternal;
    }
    //  copy of TOCRRESULTSITEMEX_EG without the Alt[] array 
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSITEMEXHDR_EG
    {
        public float Confidence;
        public System.UInt16 StructId;
        public System.UInt16 OCRCharWUnicode;        // V5 split from OCRChaW
        public System.UInt16 OCRCharWInternal;        // V5 split from OCRChaW
        public System.UInt16 FontID;
        public System.UInt16 FontStyleInfo;
        public System.UInt16 XPos;
        public System.UInt16 YPos;
        public System.UInt16 XDim;
        public System.UInt16 YDim;
        public System.UInt16 YDimRef;
        public System.UInt16 Noise;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSITEMEX_EG
    {
        public float Confidence;
        public System.UInt16 StructId;
        public System.UInt16 OCRCharWUnicode;        // V5 split from OCRChaW
        public System.UInt16 OCRCharWInternal;        // V5 split from OCRChaW
        public System.UInt16 FontID;
        public System.UInt16 FontStyleInfo;
        public System.UInt16 XPos;
        public System.UInt16 YPos;
        public System.UInt16 XDim;
        public System.UInt16 YDim;
        public System.UInt16 YDimRef;
        public System.UInt16 Noise;
        public TOCRRESULTSITEMEXALT_EG[] Alt;
        
        // N.B. this design reports the wrong structure size
        //  so we have to use careful marshaling when talking to the dll
        public void Initialize()
        {
            Alt = new TOCRRESULTSITEMEXALT_EG[5];
        }

        public void Initialize(TOCRRESULTSITEMEXHDR_EG hdr)
        {
            Confidence = hdr.Confidence;
            StructId = hdr.StructId;
            OCRCharWUnicode = hdr.OCRCharWUnicode;
            OCRCharWInternal = hdr.OCRCharWInternal;
            FontID = hdr.FontID;
            FontStyleInfo = hdr.FontStyleInfo;
            XPos = hdr.XPos;
            YPos = hdr.YPos;
            XDim = hdr.XDim;
            YDim = hdr.YDim;
            YDimRef = hdr.YDimRef;
            Noise = hdr.Noise;

            Initialize();
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSEX_EG {
        public TOCRRESULTSHEADER_EG Hdr;
        public TOCRRESULTSITEMEX_EG[] Item;
    }

#if SUPERSEDED    
    [StructLayout(LayoutKind.Sequential)]
    struct TOCRPROCESSOPTIONS {
        public System.Int32 StructId;
        public short InvertWholePage;
        public short DeskewOff;
        public byte Orientation;
        public short NoiseRemoveOff;
        public short LineRemoveOff;
        public short DeshadeOff;
        public short InvertOff;
        public short SectioningOn;
        public short MergeBreakOff;
        public short LineRejectOff;
        public short CharacterRejectOff;
        public short LexOff;
        
        [VBFixedArray(255)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=256)]
        public short[] DisableCharacter;
        
        public static void Initialize() {
            DisableCharacter = new short[256];
        }
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
    public struct TOCRJOBINFO2 {
        public System.Int32 StructId;
        public System.Int32 JobType;
        public string InputFile;
        public IntPtr hMMF;
        public System.Int32 PageNo;
        public TOCRPROCESSOPTIONS ProcessOptions;
        
        public static void Initialize() {
            ProcessOptions = new TOCRPROCESSOPTIONS();
            ProcessOptions.Initialize();
        }
    }
    
    //  Superseded by TOCRJOBINFO2
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
    public struct TOCRJOBINFO {
        public System.Int32 StructId;
        public System.Int32 JobType;
        public string InputFile;
        public System.Int32 PageNo;
        public TOCRPROCESSOPTIONS ProcessOptions;

        public static void Initialize() {
            ProcessOptions = new TOCRPROCESSOPTIONS();
            ProcessOptions.Initialize();
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSHEADER {
        public System.Int32 StructId;
        public System.Int32 XPixelsPerInch;
        public System.Int32 YPixelsPerInch;
        public System.Int32 NumItems;
        public float MeanConfidence;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSITEM {
        public short StructId;
        public short OCRCha;
        public float Confidence;
        public short XPos;
        public short YPos;
        public short XDim;
        public short YDim;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTS {
        public TOCRRESULTSHEADER Hdr;
        public TOCRRESULTSITEM[] Item;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSITEMEXALT {
        public short Valid;
        public short OCRCha;
        public float Factor;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSITEMEX {
        public short StructId;
        public short OCRCha;
        public float Confidence;
        public short XPos;
        public short YPos;
        public short XDim;
        public short YDim;
        
        [VBFixedArray(4)]
        public TOCRRESULTSITEMEXALT[] Alt;
        
        public static void Initialize() {
            Alt = new TOCRRESULTSITEMEXALT[5];
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCRRESULTSEX {
        public TOCRRESULTSHEADER Hdr;
        public TOCRRESULTSITEMEX[] Item;
    }
#endif // SUPERSEDED

    #if RESELLER1
        // Reseller Win32 and Win64 version
        const string dllfn = "TOCRRDll.dll";
    #else // not reseller so must be release version
        // Release Win32 and Win64 version
        const string dllfn = "TOCRDll.dll";
    #endif // Reseller


    [DllImport(dllfn)]
    public static extern System.Int32 TOCRInitialise(ref System.Int32 JobNo);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRShutdown(System.Int32 JobNo);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRDoJob_EG(System.Int32 JobNo, ref TOCRJOBINFO_EG JobInfo_EG);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRDoJobPDF_EG(System.Int32 JobNo, ref TOCRJOBINFO_EG JobInfo_EG, string Filename, ref TOCRPROCESSPDFOPTIONS_EG PDFOpts);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRWaitForJob(System.Int32 JobNo, ref System.Int32 JobStatus);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRWaitForAnyJob(ref System.Int32 WaitAnyStatus, ref System.Int32 JobNo);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetJobDBInfo(System.IntPtr JobSlotInf);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetJobStatus(System.Int32 JobNo, ref System.Int32 JobStatus);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetJobStatusEx2(System.Int32 JobNo, ref System.Int32 JobStatus, ref float Progress, ref System.Int32 AutoOrientation, ref System.Int32 ExtraInfFlags);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetJobStatusMsg(System.Int32 JobNo, System.Text.StringBuilder Msg);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetNumPages(System.Int32 JobNo, string Filename, System.Int32 JobType, ref System.Int32 NumPages);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetJobResultsEx_EG(System.Int32 JobNo, System.Int32 Mode, ref System.Int32 ResultsInf, System.IntPtr Bytes);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetLicenceInfoEx(System.Int32 JobNo, System.IntPtr Licence, ref System.Int32 Volume, ref System.Int32 Time, ref System.Int32 Remaining, ref System.Int32 Features);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRPopulateCharStatusMap(ref TOCRLANGUAGEOPTIONS_EG p_lang_opts, ref TOCRCHAROPTIONS_EG p_usercharvalid);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRSetConfig(System.Int32 JobNo, System.Int32 Parameter, System.Int32 Value);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetConfig(System.Int32 JobNo, System.Int32 Parameter, ref System.Int32 Value);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRConvertFormatHelper(System.Int32 JobNo, string InputAddr, System.Int32 InputFormat, string OutputAddr, System.Int32 OutputFormat, System.Int32 PageNo);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRConvertFormat(System.Int32 JobNo, string InputAddr, System.Int32 InputFormat, ref System.IntPtr OutputAddr, System.Int32 OutputFormat, System.Int32 PageNo);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRSetConfigStr(System.Int32 JobNo, System.Int32 Parameter, string Value);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetConfigStr(System.Int32 JobNo, System.Int32 Parameter, System.Text.StringBuilder Value);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetFontName(System.Int32 JobNo, System.Int32 FontID, System.Text.StringBuilder FontName);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRExtraInfGetMMF(System.Int32 JobNo, System.Int32 ExtraInfFlag, ref System.IntPtr MMF);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRTWAINAcquire(ref System.Int32 NumberOfImages);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRTWAINGetImages(System.IntPtr GlobalMemoryDIBs);
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRTWAINSelectDS();
    
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRTWAINShowUI(short Show);

#if SUPERSEDED
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRConvertFormat(System.Int32 JobNo, string InputAddr, System.Int32 InputFormat, string OutputAddr, System.Int32 OutputFormat, System.Int32 PageNo);
    
    //  Superseded by TOCRGetConfig
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetErrorMode(System.Int32 JobNo, ref System.Int32 ErrorMode);
    
    //  Superseded by TOCRSetConfig
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRSetErrorMode(System.Int32 JobNo, System.Int32 ErrorMode);
    
    //  Superseded by TOCRDoJob_EG
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRDoJob2(System.Int32 JobNo, ref TOCRJOBINFO2 JobInfo);
    
    //  Superseded by TOCRDoJob2
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRDoJob(System.Int32 JobNo, ref TOCRJOBINFO JobInfo);
    
    //  Superseded by TOCRGetJobStatusEx2
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetJobStatusEx(System.Int32 JobNo, ref System.Int32 JobStatus, ref float Progress, ref System.Int32 AutoOrientation);
    
    //  Superseded by TOCRGetJobResultsEx_EG
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetJobResults(System.Int32 JobNo, ref System.Int32 ResultsInf, System.IntPtr Bytes);
    
    //  Superseded by TOCRGetJobResultsEx_EG
    [DllImport(dllfn)]
    public static extern System.Int32 TOCRGetJobResultsEx(System.Int32 JobNo, System.Int32 Mode, ref System.Int32 ResultsInf, System.IntPtr Bytes);
#endif // SUPERSEDED

    public const short TOCRJOBMSGLENGTH = 512;
    
    //  max length of a job status message
    public const System.Int32 TOCRFONTNAMELENGTH = 65;
    
    //  max length of a returned font name
    public const System.Int32 TOCRMAXPPM = 78741;
    
    //  max pixels per metre
    public const System.Int32 TOCRMINPPM = 984;
    
    //  min pixels per metre
    //  Setting for JobNo for TOCRSetErrorMode and TOCRGetErrorMode
    public const System.Int32 TOCRDEFERRORMODE = -1;
    
    //  set/get the API error mode for all jobs
    //  Settings for ErrorMode for TOCRSetErrorMode and TOCRGetErrorMode
    public const System.Int32 TOCRERRORMODE_NONE = 0;
    
    //  API errors unseen (use return status of API calls)
    public const System.Int32 TOCRERRORMODE_MSGBOX = 1;
    
    //  API errors will bring up a message box
    public const System.Int32 TOCRERRORMODE_LOG = 2;
    
    //  errors are sent to a log file
    //  Setting for TOCRShutdown
    public const System.Int32 TOCRSHUTDOWNALL = -1;
    
    //  stop and shutdown processing for all jobs
    //  Values returned by TOCRGetJobStatus JobStatus
    public const System.Int32 TOCRJOBSTATUS_ERROR = -1;
    
    //  an error ocurred processing the last job
    public const System.Int32 TOCRJOBSTATUS_BUSY = 0;
    
    //  the job is still processing
    public const System.Int32 TOCRJOBSTATUS_DONE = 1;
    
    //  the job completed successfully
    public const System.Int32 TOCRJOBSTATUS_IDLE = 2;
    
    //  no job has been specified yet
    //  Settings for TOCRJOBINFO.JobType
    public const System.Int32 TOCRJOBTYPE_TIFFFILE = 0;
    
    //  TOCRJOBINFO.InputFile specifies a tiff file
    public const System.Int32 TOCRJOBTYPE_DIBFILE = 1;
    
    //  TOCRJOBINFO.InputFile specifies a dib (bmp) file
    public const System.Int32 TOCRJOBTYPE_DIBCLIPBOARD = 2;
    
    //  clipboard contains a dib (clipboard format CF_DIB)
    public const System.Int32 TOCRJOBTYPE_MMFILEHANDLE = 3;
    
    //  TOCRJOBINFO.PageNo specifies a handle to a memory mapped DIB file
    public const System.Int32 TOCRJOBTYPE_PDFFILE = 4;
    
    //  TOCRJOBINFO.InputFile specifies a PDF file
    //  Settings for TOCRJOBINFO.PROCESSOPTIONS.Orientation
    //  Settings for TOCRJOBINFO_EG.PROCESSOPTIONS_EG.Orientation
    public const byte TOCRJOBORIENT_AUTO = 0;
    
    //  detect orientation and rotate automatically
    public const byte TOCRJOBORIENT_OFF = 255;
    
    //  don't rotate
    public const byte TOCRJOBORIENT_90 = 1;
    
    //  90 degrees clockwise rotation
    public const byte TOCRJOBORIENT_180 = 2;
    
    //  180 degrees clockwise rotation
    public const byte TOCRJOBORIENT_270 = 3;
    
    //  270 degrees clockwise rotation
    //  Settings for TOCRJOBINFO_EG.PROCESSOPTIONS_EG.ResultsReference
    public const byte TOCRRESULTSREFERENCE_SELFREL = 0;
    
    //  relative to the first top left character recognised
    public const byte TOCRRESULTSREFERENCE_BEFORE = 1;
    
    //  page position before rotation and deskewing
    public const byte TOCRRESULTSREFERENCE_BETWEEN = 2;
    
    //  page position after rotation but before deskewing deskewing
    public const byte TOCRRESULTSREFERENCE_AFTER = 3;
    
    //  page position after rotation and deskewing
    //  Settings for TOCRJOBINFO_EG.PROCESSOPTIONS_EG.LexMode
    public const byte TOCRJOBLEXMODE_AUTO = 0;
    
    //  decide whether to apply lex
    // Public Const TOCRJOBLEXMODE_ON As Byte = 1            ' lex always on - removed for v5
    // Public Const TOCRJOBLEXMODE_OFF As Byte = 2           ' lex always off - removed for v5
    //  Settings for TOCRJOBINFO_EG.PROCESSOPTIONS_EG.Speed
    public const byte TOCRJOBSPEED_SLOW = 0;
    
    public const byte TOCRJOBSPEED_MEDIUM = 1;
    
    public const byte TOCRJOBSPEED_FAST = 2;
    
    public const byte TOCRJOBSPEED_EXPRESS = 3;
    
    //  Settings for TOCRJOBINFO_EG.PROCESSOPTIONS_EG.CCAlgorithm (Colour Conversion Algorithm)
    public const System.Int32 TOCRJOBCC_AVERAGE = 0;//  (R+G+B)/3
    public const System.Int32 TOCRJOBCC_LUMA_BT601 = 1;//  0.299*R + 0.587*G + 0.114*B
    public const System.Int32 TOCRJOBCC_LUMA_BT709 = 2;//  0.2126*R + 0.7152*G + 0.0722*B
    public const System.Int32 TOCRJOBCC_DESATURATION = 3;//  (max(R,G,B) + min(R,G,B))/2
    public const System.Int32 TOCRJOBCC_DECOMPOSITION_MAX = 4;//  max(R,G,B)
    public const System.Int32 TOCRJOBCC_DECOMPOSITION_MIN = 5;//  min(R,G,B)
    public const System.Int32 TOCRJOBCC_RED = 6; //  R
    public const System.Int32 TOCRJOBCC_GREEN = 7; //  G
    public const System.Int32 TOCRJOBCC_BLUE = 8; //  B

    // Settings for TOCRJOBINFO_EG.PROCESSOPTIONS_EG.CGAlgorithm (Conversions from Grey)
    public const System.Int32 TOCRJOBCG_HISTOGRAM = 9;

    //  Flags for TOCRJOBINFO_EG.PROCESSOPTIONS_EG.ExtraInfFlags
    public const byte TOCREXTRAINF_RETURNBITMAP1 = 1;
    
    //  Values returned by TOCRGetJobDBInfo
    public const System.Int32 TOCRJOBSLOT_FREE = 0;
    
    //  job slot is free for use
    public const System.Int32 TOCRJOBSLOT_OWNEDBYYOU = 1;
    
    //  job slot is in use by your process
    public const System.Int32 TOCRJOBSLOT_BLOCKEDBYYOU = 2;
    
    //  blocked by own process (re-initialise)
    public const System.Int32 TOCRJOBSLOT_OWNEDBYOTHER = -1;
    
    //  job slot is in use by another process (can't use)
    public const System.Int32 TOCRJOBSLOT_BLOCKEDBYOTHER = -2;
    
    //  blocked by another process (can't use)
    //  Values returned in WaitAnyStatus by TOCRWaitForAnyJob
    public const System.Int32 TOCRWAIT_OK = 0;
    
    //  JobNo is the job that finished (get and check it's JobStatus)
    public const System.Int32 TOCRWAIT_SERVICEABORT = 1;
    
    //  JobNo is the job that failed (re-initialise)
    public const System.Int32 TOCRWAIT_CONNECTIONBROKEN = 2;
    
    //  JobNo is the job that failed (re-initialise)
    public const System.Int32 TOCRWAIT_FAILED = -1;
    
    //  JobNo not set - check manually
    public const System.Int32 TOCRWAIT_NOJOBSFOUND = -2;
    
    //  JobNo not set - no running jobs found
    //  Settings for Mode for TOCRGetJobResultsEx
    public const System.Int32 TOCRGETRESULTS_NORMAL = 0;
    
    //  return results for TOCRRESULTS
    public const System.Int32 TOCRGETRESULTS_EXTENDED = 1;
    
    //  return results for TOCRRESULTSEX
    //  Settings for Mode for TOCRGetJobResultsEx_EG
    public const System.Int32 TOCRGETRESULTS_NORMAL_EG = 2;
    
    //  return results for TOCRRESULTS_EG
    public const System.Int32 TOCRGETRESULTS_EXTENDED_EG = 3;
    
    //  return results for TOCRRESULTSEX_EG
    //  Values returned in ResultsInf by TOCRGetJobResults and TOCRGetJobResultsEx
    public const System.Int32 TOCRGETRESULTS_NORESULTS = -1;
    
    //  no results are available
    //  Flags returned by TOCRResults_EG.Item().FontStyleInfo
    //  Flags returned by TOCRResultsEx_EG.Item().FontStyleInfo
    public const System.UInt16 TOCRRESULTSFONT_NOTSET = 0;
    
    //  character tyle is not specified
    public const System.UInt16 TOCRRESULTSFONT_NORMAL = 1;
    
    //  character is Normal
    // Public Const TOCRRESULTSFONT_BOLD As System.UInt16 = 2     ' character is Bold - removed for v5
    public const System.UInt16 TOCRRESULTSFONT_ITALIC = 2;
    
    //  character is Italic
    public const System.UInt16 TOCRRESULTSFONT_UNDERLINE = 4;
    
    //  character is Underlined
    //  Values for TOCRConvertFormat InputFormat
    public const System.Int32 TOCRCONVERTFORMAT_TIFFFILE = TOCRJOBTYPE_TIFFFILE;
    
    public const System.Int32 TOCRCONVERTFORMAT_PDFFILE = TOCRJOBTYPE_PDFFILE;
    
    //  Values for TOCRConvertFormat OutputFormat
    public const System.Int32 TOCRCONVERTFORMAT_DIBFILE = TOCRJOBTYPE_DIBFILE;
    
    public const System.Int32 TOCRCONVERTFORMAT_MMFILEHANDLE = TOCRJOBTYPE_MMFILEHANDLE;
    
    //  Values for licence features (returned by TOCRGetLicenceInfoEx)
    public const System.Int32 TOCRLICENCE_STANDARD = 1;
    
    //  standard licence (no higher characters)
    public const System.Int32 TOCRLICENCE_EURO = 2;
    
    //  higher characters
    public const System.Int32 TOCRLICENCE_EUROUPGRADE = 3;
    
    //  standard licence upgraded to euro
    public const System.Int32 TOCRLICENCE_V3SE = 4;
    
    //  V3SE version 3 standard edition licence (no API)
    public const System.Int32 TOCRLICENCE_V3SEUPGRADE = 5;
    
    //  versions 1/2 upgraded to V3 standard edition (no API)
    //  Note V4 licences are the same as V3 Pro licences
    public const System.Int32 TOCRLICENCE_V3PRO = 6;
    
    //  V3PRO version 3 pro licence
    public const System.Int32 TOCRLICENCE_V3PROUPGRADE = 7;
    
    //  versions 1/2 upgraded to version 3 pro
    public const System.Int32 TOCRLICENCE_V3SEPROUPGRADE = 8;
    
    //  version 3 standard edition upgraded to version 3 pro
    public const System.Int32 TOCRLICENCE_V5 = 9;
    
    //  version 5
    public const System.Int32 TOCRLICENCE_V5UPGRADE3 = 10;
    
    //  version 5 upgraded from version 3
    public const System.Int32 TOCRLICENCE_V5UPGRADE12 = 11;
    
    //  version 5 upgraded from version 1/2
    //  Values for TOCRSetConfig and TOCRGetConfig
    public const System.Int32 TOCRCONFIG_DEFAULTJOB = -1;
    
    //  default job number (all new jobs)
    public const System.Int32 TOCRCONFIG_DLL_ERRORMODE = 0;
    
    //  set the dll ErrorMode
    public const System.Int32 TOCRCONFIG_SRV_ERRORMODE = 1;
    
    //  set the service ErrorMode
    public const System.Int32 TOCRCONFIG_SRV_THREADPRIORITY = 2;
    
    //  set the service thread priority
    public const System.Int32 TOCRCONFIG_DLL_MUTEXWAIT = 3;
    
    //  set the dll mutex wait timeout (ms)
    public const System.Int32 TOCRCONFIG_DLL_EVENTWAIT = 4;
    
    //  set the dll event wait timeout (ms)
    public const System.Int32 TOCRCONFIG_SRV_MUTEXWAIT = 5;
    
    //  set the service mutex wait timeout (ms)
    public const System.Int32 TOCRCONFIG_LOGFILE = 6;
    
    public const System.Int32 TOCR_OK = 0;
    
    // Public Const TOCRERR_ILLEGALJOBNO As Integer = 1
    // Public Const TOCRERR_FAILLOCKDB As Integer = 2
    // Public Const TOCRERR_NOFREEJOBSLOTS As Integer = 3
    // Public Const TOCRERR_FAILSTARTSERVICE As Integer = 4
    // Public Const TOCRERR_FAILINITSERVICE As Integer = 5
    // Public Const TOCRERR_JOBSLOTNOTINIT As Integer = 6
    // Public Const TOCRERR_JOBSLOTINUSE As Integer = 7
    // Public Const TOCRERR_SERVICEABORT As Integer = 8
    // Public Const TOCRERR_CONNECTIONBROKEN As Integer = 9
    // Public Const TOCRERR_INVALIDSTRUCTID As Integer = 10
    // Public Const TOCRERR_FAILGETVERSION As Integer = 11
    // Public Const TOCRERR_FAILLICENCEINF As Integer = 12
    // Public Const TOCRERR_LICENCEEXCEEDED As Integer = 13
    // Public Const TOCRERR_MISMATCH As Integer = 15
    // Public Const TOCRERR_JOBSLOTNOTYOURS As Integer = 16
    // Public Const TOCRERR_FAILGETJOBSTATUS1 As Integer = 20
    // Public Const TOCRERR_FAILGETJOBSTATUS2 As Integer = 21
    // Public Const TOCRERR_FAILGETJOBSTATUS3 As Integer = 22
    // Public Const TOCRERR_FAILCONVERT As Integer = 23
    // Public Const TOCRERR_FAILSETCONFIG As Integer = 24
    // Public Const TOCRERR_FAILGETCONFIG As Integer = 25
    // Public Const TOCRERR_FAILGETJOBSTATUS4 As Integer = 26
    // Public Const TOCRERR_FAILDOJOB1 As Integer = 30
    // Public Const TOCRERR_FAILDOJOB2 As Integer = 31
    // Public Const TOCRERR_FAILDOJOB3 As Integer = 32
    // Public Const TOCRERR_FAILDOJOB4 As Integer = 33
    // Public Const TOCRERR_FAILDOJOB5 As Integer = 34
    // Public Const TOCRERR_FAILDOJOB6 As Integer = 35
    // Public Const TOCRERR_FAILDOJOB7 As Integer = 36
    // Public Const TOCRERR_FAILDOJOB8 As Integer = 37
    // Public Const TOCRERR_FAILDOJOB9 As Integer = 38
    // Public Const TOCRERR_FAILDOJOB10 As Integer = 39
    // Public Const TOCRERR_UNKNOWNJOBTYPE1 As Integer = 40
    // Public Const TOCRERR_JOBNOTSTARTED1 As Integer = 41
    // Public Const TOCRERR_FAILDUPHANDLE As Integer = 42
    // Public Const TOCRERR_FAILGETJOBSTATUSMSG1 As Integer = 45
    // Public Const TOCRERR_FAILGETJOBSTATUSMSG2 As Integer = 46
    // Public Const TOCRERR_FAILGETNUMPAGES1 As Integer = 50
    // Public Const TOCRERR_FAILGETNUMPAGES2 As Integer = 51
    // Public Const TOCRERR_FAILGETNUMPAGES3 As Integer = 52
    // Public Const TOCRERR_FAILGETNUMPAGES4 As Integer = 53
    // Public Const TOCRERR_FAILGETNUMPAGES5 As Integer = 54
    // Public Const TOCRERR_FAILGETRESULTS1 As Integer = 60
    // Public Const TOCRERR_FAILGETRESULTS2 As Integer = 61
    // Public Const TOCRERR_FAILGETRESULTS3 As Integer = 62
    // Public Const TOCRERR_FAILGETRESULTS4 As Integer = 63
    // Public Const TOCRERR_FAILALLOCMEM100 As Integer = 64
    // Public Const TOCRERR_FAILALLOCMEM101 As Integer = 65
    // Public Const TOCRERR_FILENOTSPECIFIED As Integer = 66
    // Public Const TOCRERR_INPUTNOTSPECIFIED As Integer = 67
    // Public Const TOCRERR_OUTPUTNOTSPECIFIED As Integer = 68
    // Public Const TOCRERR_INVALIDPARAMETER As Integer = 69
    // Public Const TOCRERR_FAILROTATEBITMAP As Integer = 70
    public const System.Int32 TOCERR_TWAINPARTIALACQUIRE = 80;
    
    // Public Const TOCERR_TWAINFAILEDACQUIRE As Integer = 81
    // Public Const TOCERR_TWAINNOIMAGES As Integer = 82
    // Public Const TOCERR_TWAINSELECTDSFAILED As Integer = 83
    // Public Const TOCERR_MMFNOTALLOWED As Integer = 84
    // Public Const TOCRERR_ILLEGALFONTID As Integer = 85
    // Public Const TOCRERR_FAILGETMMF As Integer = 90
    // Public Const TOCRERR_MMFNOTAVAILABLE As Integer = 91
    public const System.Int32 TOCRERR_PDFEXTRACTOR = 95;
    
    public const System.Int32 TOCRERR_PDFERROR2 = 96;
    
    public const System.Int32 TOCRERR_PDFARCHIVER = 97;
}