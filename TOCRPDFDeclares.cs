
//using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
//***************************************************************************
// Module:     TOCRPDFDeclares
//
// TOCRPDF declares - part of TOCR Version 5.1.0.0

//#Const SUPERSEDED = False ' disallow superseded routines

//
//
//
//
//
//
//
//            CHANGED ALL LONGs to INTEGERs
//            THAT INCLUDES THE ULONG to UINTEGER IN CHARPTR_WITH_LEN
//
//            CHANGED ALL BOOLEANs TO USHORTs to MATCH DLL
//			??? VBBOOL is defined as a signed short - so I have used Short for now
//			??? VB now has system.Boolean - is that prefered and what is the 'c' equivilent type?
//
//
//



using System.Runtime.InteropServices;


static class TOCRPDFDeclares
{
    public const Int32 _MAX_PATH = 260;

    #region " Boolean Values "
    public const byte VARIANT_TRUE = 255; //-1
    public const byte VARIANT_FALSE = 0;
    #endregion

    #region " Enums "

    public enum TOCRPDF_Error : short
    {
     TOCRPDF_ErrorOK = 0,					/** The default value indicating no error. */
     TOCRPDF_PoDoFo_Exception = 1,			/** PoDoFo::PdfError       */
     TOCRPDF_Standard_Exception = 2,		/** std::exception */
     TOCRPDF_Unknown_Error = 3,				/** catch(...) */
     TOCRPDF_Invalid_PDFExtractor = 4,		/** PDFExtractor is NULL */
     TOCRPDF_Invalid_PDFArchiver = 5,		/** PDFArchiver is NULL */
     TOCRPDF_DLL_not_loaded = 6,			/** function in a delay-loaded dll is NULL */
    }

    public enum TOCRPDF_COLOUR_MODE : short
    {
     COLOUR_MODE_MONO = 0,
     COLOUR_MODE_GREY8 = 1,
     COLOUR_MODE_RGB24 = 2,
    }

    public enum TocrResultStage : short
	{
		TRS_Extra_Text_Page = 500,
		// there was too much text to fit on one page so this page was created to accomodate the excess
		TRS_Text = 400,
		TRS_Processed_Image = 300,
		TRS_Input_Image = 200,
		TRS_Original_Page = 100
	}
   	#endregion

	#region " Structures "
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CHARPTR_WITH_LEN
	{
		public IntPtr charPtr;
        public UInt32 len;
	}

    [StructLayout(LayoutKind.Sequential)]
	public struct PDFExtractorHandle
	{
		public IntPtr DataHandle;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PDFExtractorMemDocHandle
	{
		public IntPtr DataHandle;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PDFExtractorPageHandle
	{
		public IntPtr DataHandle;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PageSize
	{
		public double width;
		public double height;
	}

	[StructLayout(LayoutKind.Sequential, Pack=4, CharSet = CharSet.Unicode)]
	public struct TocrResultsInfo
	{
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = _MAX_PATH)]
        public char[] OriginalFileName;
        public UInt64 OriginalPageNumber;
        private UInt32 OutputStageInternal;
        public byte bContainedInThisDocument; // please use TOCRPDFDeclares.VARIANT_TRUE or TOCRPDFDeclares.VARIANT_FALSE
        
        public void Initialize()
        {
            OriginalFileName = new char[_MAX_PATH];
        }

        public TocrResultStage OutputStage
        {
            get { return (TocrResultStage)OutputStageInternal; }
            set { OutputStageInternal = (UInt32)value; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DpiPair
	{
		public double DpiX;
		public double DpiY;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PDFArchiverHandle
	{
		public IntPtr DataHandle;
	}

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PDFArchiverMemDocHandle
	{
		public IntPtr DataHandle;
	}

	#endregion

	#region " Declares "
	// 3 WAYS TO DECLARE A STRING FUNCTION
	//Declare Function testfn Lib "TOCRDLL" _
	//(<MarshalAs(UnmanagedType.LPWStr)> ByVal UniStr As String, ByVal ANsiStr As String, ByVal lens2 As Integer, ByRef ju As TOCRJOBINFO_EG) As Integer
	//Declare Ansi Function testfn Lib "TOCRDLL" _
	//(<MarshalAs(UnmanagedType.LPWStr)> ByVal UniStr As String, ByVal ANsiStr As String, ByVal lens2 As Integer, ByRef ju As TOCRJOBINFO_EG) As Integer
	//Declare Unicode Function testfn Lib "TOCRDLL" _
	//(ByVal UniStr As String, <MarshalAs(UnmanagedType.LPStr)> ByVal ANsiStr As String, ByVal lens2 As Integer, ByRef ju As TOCRJOBINFO_EG) As Integer


#if Win64
        // Release and Win64 version
        const string dllfn = "TOCRPDF64.dll";
#else // not reseller so must be release version
        // Release and Win32 version
        const string dllfn = "TOCRPDF32.dll";
#endif // Reseller


    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFExtractorHandle_New(ref PDFExtractorHandle pExtractor);
	[DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
	public static extern UInt32 PDFExtractorHandle_Delete(PDFExtractorHandle Extractor);
	
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
	public static extern UInt32 PDFExtractorMemDocHandle_New(ref PDFExtractorMemDocHandle pMemDoc);
	[DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
	public static extern UInt32 PDFExtractorMemDocHandle_Delete(PDFExtractorMemDocHandle MemDoc);
	
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFExtractorPageHandle_New(ref PDFExtractorPageHandle pPage);
	[DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
	public static extern UInt32 PDFExtractorPageHandle_Delete(PDFExtractorPageHandle Page);
	
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
#if Win64
	public static extern UInt32 PDFExtractorHandle_Load(PDFExtractorHandle Extractor, PDFExtractorMemDocHandle MemDoc, Int64 AddressOfFirstCharacterOfInFileName);
#else
    public static extern UInt32 PDFExtractorHandle_Load(PDFExtractorHandle Extractor, PDFExtractorMemDocHandle MemDoc, Int32 AddressOfFirstCharacterOfInFileName);
#endif

	[DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
	public static extern UInt32 PDFExtractorHandle_GetPageCount(PDFExtractorHandle Extractor, PDFExtractorMemDocHandle MemDoc, ref UInt32 pCount);
	[DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
	public static extern UInt32 PDFExtractorHandle_FindPageSize(PDFExtractorHandle Extractor, PDFExtractorPageHandle Page, ref double pWidth, ref double pHeight);
	[DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFExtractorHandle_GetTocrImageInfo(PDFExtractorHandle Extractor, PDFExtractorPageHandle Page, ref UInt32/*TOCRPDF_COLOUR_MODE*/ ColourMode, ref double pDpiX, ref double pDpiY);
	[DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
	public static extern UInt32 PDFExtractorHandle_GetPage(PDFExtractorHandle Extractor, PDFExtractorMemDocHandle MemDoc, PDFExtractorPageHandle Page, UInt32 nPage);
	
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFExtractorHandle_GetRecommendedDPIForPageSize(PDFExtractorHandle Extractor, double width, double height, UInt32/*TOCRPDF_COLOUR_MODE*/ ColourMode, ref double pDpiX, ref double pDpiY);

    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFExtractorHandle_PageToDibMem(PDFExtractorHandle Extractor, PDFExtractorPageHandle Page, ref CHARPTR_WITH_LEN p_cpwl, UInt32/*TOCRPDF_COLOUR_MODE*/ ColourMode, double pDpiX, double pDpiY, ref Int16 pPageIsNotBlank);

    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
#if Win64
    public static extern UInt32 PDFExtractorHandle_PageToDibFile(PDFExtractorHandle Extractor, PDFExtractorPageHandle Page, Int64 AddressOfFirstCharacterOfOutFileName, UInt32/*TOCRPDF_COLOUR_MODE*/ ColourMode, double pDpiX, double pDpiY, ref Int16 pPageIsNotBlank);
#else
    public static extern UInt32 PDFExtractorHandle_PageToDibFile(PDFExtractorHandle Extractor, PDFExtractorPageHandle Page, Int32 AddressOfFirstCharacterOfOutFileName, UInt32/*TOCRPDF_COLOUR_MODE*/ ColourMode, double pDpiX, double pDpiY, ref Int16 pPageIsNotBlank);
#endif

    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
#if Win64
    public static extern UInt32 PDFExtractorHandle_AddAppendix(PDFExtractorHandle Extractor, Int64 AddressOfFirstCharacterOfInFileName, Int64 AddressOfFirstCharacterOfOutFileName, Int64 AddressOfFirstCharacterOfApendix);
#else
    public static extern UInt32 PDFExtractorHandle_AddAppendix(PDFExtractorHandle Extractor, Int32 AddressOfFirstCharacterOfInFileName, Int32 AddressOfFirstCharacterOfOutFileName, Int32 AddressOfFirstCharacterOfApendix);
#endif
	
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
#if Win64
    public static extern UInt32 PDFExtractorHandle_AddAppendixEx(PDFExtractorHandle Extractor, Int64 AddressOfFirstCharacterOfInFileName, Int64 AddressOfFirstCharacterOfOutFileName, TOCRDeclares.TOCRRESULTSEX_EG Appendix, Int64 AddressOfFirstCharacterOfTitle, double width, double height, double dpiX, double dpiY, ref TocrResultsInfo ResultsInfo);
#else
    public static extern UInt32 PDFExtractorHandle_AddAppendixEx(PDFExtractorHandle Extractor, Int32 AddressOfFirstCharacterOfInFileName, Int32 AddressOfFirstCharacterOfOutFileName, TOCRDeclares.TOCRRESULTSEX_EG Appendix, Int32 AddressOfFirstCharacterOfTitle, double width, double height, double dpiX, double dpiY, ref TocrResultsInfo ResultsInfo);
#endif

    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFExtractorHandle_GetLastExceptionText(PDFExtractorHandle Extractor, UInt32 err, ref CHARPTR_WITH_LEN p_cpwl);

    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFArchiverHandle_New(ref PDFArchiverHandle pArchiver);
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFArchiverHandle_Delete(PDFArchiverHandle Archiver);
    
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFArchiverMemDocHandle_New(ref PDFArchiverMemDocHandle pMemDoc);
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFArchiverMemDocHandle_Delete(PDFArchiverMemDocHandle MemDoc);

    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
#if Win64
    public static extern UInt32 PDFArchiverHandle_Load(PDFArchiverHandle Archiver, PDFArchiverMemDocHandle MemDoc, Int64 AddressOfFirstCharacterOfInFileName);
#else
    public static extern UInt32 PDFArchiverHandle_Load(PDFArchiverHandle Archiver, PDFArchiverMemDocHandle MemDoc, Int32 AddressOfFirstCharacterOfInFileName);
#endif
    
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFArchiverHandle_SaveAllPdf(PDFArchiverHandle Archiver, PDFArchiverMemDocHandle hDoc, PDFArchiverMemDocHandle hDocIn, Int32 ResultsPageNumber, ref TocrResultsInfo ResultsInfo);

    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
#if Win64
    public static extern UInt32 PDFArchiverHandle_SaveAppendix(PDFArchiverHandle hArchiver, PDFArchiverMemDocHandle hDoc, Int64 AddressOfFirstCharacterOfAppendix, Int32 ResultsPageNumber, ref TocrResultsInfo ResultsInfo);
#else
    public static extern UInt32 PDFArchiverHandle_SaveAppendix(PDFArchiverHandle hArchiver, PDFArchiverMemDocHandle hDoc, Int32 AddressOfFirstCharacterOfAppendix, Int32 ResultsPageNumber, ref TocrResultsInfo ResultsInfo);
#endif
    
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
#if Win64
    public static extern UInt32 PDFArchiverHandle_Close(PDFArchiverHandle hArchiver, PDFArchiverMemDocHandle hDoc, Int64 AddressOfFirstCharacterOfOutFileName);
#else
    public static extern UInt32 PDFArchiverHandle_Close(PDFArchiverHandle hArchiver, PDFArchiverMemDocHandle hDoc, Int32 AddressOfFirstCharacterOfOutFileName);
#endif
    
    [DllImport(dllfn, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern UInt32 PDFArchiverHandle_GetLastExceptionText(PDFArchiverHandle Archiver, UInt32 err, ref CHARPTR_WITH_LEN p_cpwl);
    #endregion

	#region " Error Codes "
		// The default value indicating no error.
	public const int TOCRPDF_ErrorOK = 0;
		// PoDoFo::PdfError
	public const int TOCRPDF_PoDoFo_Exception = 1;
		// std::exception
	public const int TOCRPDF_Standard_Exception = 2;
		// catch(...)
	public const int TOCRPDF_Unknown_Error = 3;
		// PDFExtractorHandle is NULL
	public const int TOCRPDF_Invalid_PDFExtractor = 4;
		// PDFArchiverHandle is NULL
	public const int TOCRPDF_Invalid_PDFArchiver = 5;
	#endregion

    #region " Wrapper Classes "
    #region " PdfBase "
	public abstract class PDFBase : IDisposable
	{
        public virtual string GetLastExceptionText(UInt32 TPRes)
		{
			string msg = null;

			if (TOCRPDF_ErrorOK == TPRes) {
				msg = "OK";
			} else if (TOCRPDF_PoDoFo_Exception == TPRes) {
				msg = "PoDoFo Exception";
			} else if (TOCRPDF_Standard_Exception == TPRes) {
				msg = "Standard Exception";
			} else if (TOCRPDF_Invalid_PDFExtractor == TPRes) {
				msg = "Invalid PDFExtractor handle used";
            } else if (TOCRPDF_Invalid_PDFArchiver == TPRes) {
				msg = "Invalid PDFArchiver handle used";
			} else {
				msg = "Unknown error";
			}

			return msg;
		}

        public abstract UInt32 TocrPdfResult(UInt32 TPRes);

		#region " IDisposable Support "
		// Keep track of when the object is disposed. 

		protected bool disposed = false;
		// This method disposes the base object's resources. 
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed) {
				if (disposing) {
					// Insert code to free managed resources. 
				}
				// Insert code to free unmanaged resources. 
			}
			this.disposed = true;
		}

		// Do not change or add Overridable to these methods. 
		// Put cleanup code in Dispose(ByVal disposing As Boolean). 
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

        // Use C# destructor syntax for finalization code.
        ~PDFBase()
		{
			Dispose(false);
			// base.Finalize(); // NP - c# calls this for us at the right time
		}
		#endregion

	}
    #endregion

    #region " Extractor "
    public class PDFExtractor : PDFExtractorBase
	{
		public PDFExtractorHandle m_hExtractor;
		public PDFExtractor()
		{
            m_hExtractor = new PDFExtractorHandle();

            TocrPdfResult(PDFExtractorHandle_New(ref m_hExtractor));
		}

		public PDFExtractorMemDoc Load(string inFileName)
		{
			PDFExtractorMemDoc pMemDoc = new PDFExtractorMemDoc(this);
			pMemDoc.Load(inFileName);
			return pMemDoc;
		}

		public void AddAppendix(string inFileName, string outFileName, string Appendix)
		{
			GCHandle hInFileName = default(GCHandle);
			System.IntPtr pInFileName = default(System.IntPtr);

			hInFileName = GCHandle.Alloc(inFileName, GCHandleType.Pinned);
			pInFileName = hInFileName.AddrOfPinnedObject();
			#if Win64
			Int64 iInFileName = default(Int64);
			iInFileName = pInFileName.ToInt64();
			#else // not Win64
			Int32 iInFileName = default(Int32);
			iInFileName = pInFileName.ToInt32();
			#endif // Win64

			GCHandle hOutFileName = default(GCHandle);
			System.IntPtr pOutFileName = default(System.IntPtr);
			hOutFileName = GCHandle.Alloc(outFileName, GCHandleType.Pinned);
			pOutFileName = hOutFileName.AddrOfPinnedObject();
			#if Win64
			Int64 iOutFileName = default(Int64);
			iOutFileName = pOutFileName.ToInt64();
			#else // not Win64
			Int32 iOutFileName = default(Int32);
			iOutFileName = pOutFileName.ToInt32();
			#endif // Win64

			GCHandle hAppendix = default(GCHandle);
			System.IntPtr pAppendix = default(System.IntPtr);
			hAppendix = GCHandle.Alloc(Appendix, GCHandleType.Pinned);
			pAppendix = hAppendix.AddrOfPinnedObject();
			#if Win64
			Int64 iAppendix = default(Int64);
			iAppendix = pAppendix.ToInt64();
			#else // not Win64
			Int32 iAppendix = default(Int32);
			iAppendix = pAppendix.ToInt32();
			#endif // Win64

			TocrPdfResult(PDFExtractorHandle_AddAppendix(m_hExtractor, iInFileName, iOutFileName, iAppendix));
		}


        public DpiPair GetRecommendedDPI(PageSize size, TOCRPDF_COLOUR_MODE ColourMode)
		{
			DpiPair pDpi = default(DpiPair);

			TocrPdfResult(PDFExtractorHandle_GetRecommendedDPIForPageSize(m_hExtractor, size.width, size.height, (UInt32)ColourMode, ref pDpi.DpiX, ref pDpi.DpiY));

			return pDpi;
		}

		public override string GetLastExceptionText(UInt32 TPRes)
		{
			string msg = null;

			if ((TOCRPDF_PoDoFo_Exception == TPRes) | (TOCRPDF_Standard_Exception == TPRes)) {
				CHARPTR_WITH_LEN p_cpwl = default(CHARPTR_WITH_LEN);
				byte[] buf = null;
				buf = new byte[1001];

				GCHandle hBuf = default(GCHandle);
				hBuf = GCHandle.Alloc(buf, GCHandleType.Pinned);

				p_cpwl.charPtr = hBuf.AddrOfPinnedObject();
				p_cpwl.len = 1000;

                UInt32 innerTPRes = PDFExtractorHandle_GetLastExceptionText(m_hExtractor, TPRes, ref p_cpwl);
				hBuf.Free();

				if ((TOCRPDF_ErrorOK == innerTPRes)) {
					msg = System.Text.Encoding.ASCII.GetString(buf);
				} else if ((TOCRPDF_PoDoFo_Exception == innerTPRes)) {
					msg = "Unknown PoDoFo Exception";
				} else if ((TOCRPDF_Standard_Exception == innerTPRes)) {
					msg = "Unknown Standard Exception";
				} else {
					msg = base.GetLastExceptionText(innerTPRes);
				}
			} else {
				msg = base.GetLastExceptionText(TPRes);
			}

			return msg;

		}

        public override UInt32 TocrPdfResult(UInt32 TPRes)
		{
			if ((TOCRPDF_ErrorOK != TPRes)) {
				// ask the dll for the error msg - and later ask for the stack trace
				string msg = GetLastExceptionText(TPRes);

				Exception innerException = new Exception(msg);
				PDFExtractorException ex = new PDFExtractorException("TOCR PDF DLL is not able to do that. Error " + TPRes, innerException);
				throw (ex);
				//MsgBox("TOCR PDF DLL is not able to do that. Error " & TPRes, MsgBoxStyle.Exclamation)
			}

			return TPRes;
		}

		#region " IDisposable Support "
		// This method disposes the derived object's resources. 
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed) {
				if (disposing) {
					// Insert code to free managed resources. 
				}
				// Insert code to free unmanaged resources. 
				TocrPdfResult(PDFExtractorHandle_Delete(m_hExtractor));
			}
			base.Dispose(disposing);
		}

		// The derived class does not have a Finalize method 
		// or a Dispose method with parameters because it inherits 
		// them from the base class. 
		#endregion

	}

	public class PDFExtractorBase : PDFBase
	{
        public override UInt32 TocrPdfResult(UInt32 TPRes)
		{
			if ((TOCRPDF_ErrorOK != TPRes)) {
				PDFExtractorException ex = new PDFExtractorException("TOCR PDF DLL is not able to do that. Error " + TPRes);
				throw (ex);
				//MsgBox("TOCR PDF DLL is not able to do that. Error " & TPRes, MsgBoxStyle.Exclamation)
			}

			return TPRes;
		}

		#region " IDisposable Support "
		// This method disposes the derived object's resources. 
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed) {
				if (disposing) {
					// Insert code to free managed resources. 
				}
				// Insert code to free unmanaged resources. 
			}
			base.Dispose(disposing);
		}

		// The derived class does not have a Finalize method 
		// or a Dispose method with parameters because it inherits 
		// them from the base class. 
		#endregion


	}

	public class PDFExtractorUser : PDFExtractorBase
	{
        public PDFExtractor m_pExtractor;
		protected PDFExtractorUser()
		{
			// don't call this as it would leave us with an unpopulated PDFExtractor
		}

		public PDFExtractorUser(PDFExtractor pExtractor)
		{
			m_pExtractor = pExtractor;
		}

        public DpiPair GetRecommendedDPI(PageSize size, TOCRPDF_COLOUR_MODE ColourMode)
		{
			DpiPair pDPI = default(DpiPair);
			pDPI = m_pExtractor.GetRecommendedDPI(size, ColourMode);

			return pDPI;
		}

        public override string GetLastExceptionText(UInt32 TPRes)
		{
			return m_pExtractor.GetLastExceptionText(TPRes);
		}

        public override UInt32 TocrPdfResult(UInt32 TPRes)
		{
			if ((TOCRPDF_ErrorOK != TPRes)) {
				TPRes = m_pExtractor.TocrPdfResult(TPRes);
			}

			return TPRes;
		}


		#region " IDisposable Support "
		// This method disposes the derived object's resources. 
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed) {
				if (disposing) {
					// Insert code to free managed resources. 
				}
				// Insert code to free unmanaged resources. 
			}
			base.Dispose(disposing);
		}

		// The derived class does not have a Finalize method 
		// or a Dispose method with parameters because it inherits 
		// them from the base class. 
		#endregion

	}

	public class PDFExtractorMemDoc : PDFExtractorUser
	{
		public PDFExtractorMemDocHandle m_hMemDoc;
		protected PDFExtractorMemDoc()
		{
			// don't call this as it would leave us with an unpopulated PDFExtractor
		}

		public PDFExtractorMemDoc(PDFExtractor pExtractor) : base(pExtractor)
		{
			TocrPdfResult(PDFExtractorMemDocHandle_New(ref m_hMemDoc));
		}

		public void Load(string inFileName)
		{
			GCHandle hInFileName = default(GCHandle);
			System.IntPtr pInFileName = default(System.IntPtr);
			hInFileName = GCHandle.Alloc(inFileName, GCHandleType.Pinned);
			pInFileName = hInFileName.AddrOfPinnedObject();
			#if Win64
			Int64 iInFileName = default(Int64);
			iInFileName = pInFileName.ToInt64();
			#else // not Win64
			Int32 iInFileName = default(Int32);
			iInFileName = pInFileName.ToInt32();
			#endif // Win64

			TocrPdfResult(PDFExtractorHandle_Load(m_pExtractor.m_hExtractor, m_hMemDoc, iInFileName));
		}

		public uint GetPageCount()
		{
			uint count = 0;
			TocrPdfResult(PDFExtractorHandle_GetPageCount(m_pExtractor.m_hExtractor, m_hMemDoc, ref count));

			return count;
		}

		public PDFExtractorPage GetPage(uint nPage)
		{
			PDFExtractorPage pPage = new PDFExtractorPage(m_pExtractor);

			TocrPdfResult(PDFExtractorHandle_GetPage(m_pExtractor.m_hExtractor, m_hMemDoc, pPage.m_hPage, nPage));

			return pPage;
		}

		#region " IDisposable Support "
		// This method disposes the derived object's resources. 
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed) {
				if (disposing) {
					// Insert code to free managed resources. 
				}
				// Insert code to free unmanaged resources. 
				TocrPdfResult(PDFExtractorMemDocHandle_Delete(m_hMemDoc));
			}
			base.Dispose(disposing);
		}

		// The derived class does not have a Finalize method 
		// or a Dispose method with parameters because it inherits 
		// them from the base class. 
		#endregion

	}

	public class PDFExtractorPage : PDFExtractorUser
	{

		public PDFExtractorPageHandle m_hPage;
		public bool m_PageIsNotBlank;
        public TOCRPDF_COLOUR_MODE m_ColourMode;
		public DpiPair m_DPI;

		public PageSize m_size;
		protected PDFExtractorPage()
		{
			// don't call this as it would leave us with an unpopulated PDFExtractor
		}

		public PDFExtractorPage(PDFExtractor pExtractor) : base(pExtractor)
		{
			TocrPdfResult(PDFExtractorPageHandle_New(ref m_hPage));
			m_PageIsNotBlank = false;
            m_ColourMode = TOCRPDF_COLOUR_MODE.COLOUR_MODE_GREY8;
			// Grey8
			m_DPI.DpiX = 0;
			m_DPI.DpiY = 0;
			m_size.width = 0;
			m_size.height = 0;
		}

		public PageSize FindPageSize()
		{
			PageSize size = new PageSize();

			TocrPdfResult(PDFExtractorHandle_FindPageSize(m_pExtractor.m_hExtractor, m_hPage, ref size.width, ref size.height));

			return size;
		}

		public void PageToDib(CHARPTR_WITH_LEN p_cpwl)
		{
			short PageIsNotBlank = 0;

			if (((m_DPI.DpiX == 0) & (m_DPI.DpiY == 0))) {
				PageSize size = new PageSize();
				size = FindPageSize();
				m_DPI = GetRecommendedDPI(size, m_ColourMode);
			}

			TocrPdfResult(PDFExtractorHandle_PageToDibMem(m_pExtractor.m_hExtractor, m_hPage, ref p_cpwl, (UInt32)m_ColourMode, m_DPI.DpiX, m_DPI.DpiY, ref PageIsNotBlank));

			if ((TOCRPDFDeclares.VARIANT_FALSE == PageIsNotBlank)) {
				m_PageIsNotBlank = false;
			} else {
				m_PageIsNotBlank = true;
			}
		}

		#region " IDisposable Support "
		// This method disposes the derived object's resources. 
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed) {
				if (disposing) {
					// Insert code to free managed resources. 
				}
				// Insert code to free unmanaged resources. 
				TocrPdfResult(PDFExtractorPageHandle_Delete(m_hPage));
			}
			base.Dispose(disposing);
		}

		// The derived class does not have a Finalize method 
		// or a Dispose method with parameters because it inherits 
		// them from the base class. 
		#endregion

	}

	public class PDFExtractorException : ApplicationException
	{

		public PDFExtractorException() : base()
		{
		}

		public PDFExtractorException(string message) : base(message)
		{
		}

		public PDFExtractorException(string message, System.Exception innerException) : base(message, innerException)
		{
		}

	}

    #endregion

    #region " Archiver "
    public class PDFArchiver : PDFArchiverBase
    {
        public PDFArchiverHandle m_hArchiver;
        public PDFArchiver()
        {
            TocrPdfResult(PDFArchiverHandle_New(ref m_hArchiver));
        }

        public PDFArchiverMemDoc Load(string inFileName)
        {
            PDFArchiverMemDoc pMemDoc = new PDFArchiverMemDoc(this);
            pMemDoc.Load(inFileName);
            return pMemDoc;
        }

        public override string GetLastExceptionText(UInt32 TPRes)
        {
            string msg = null;

            if ((TOCRPDF_PoDoFo_Exception == TPRes) | (TOCRPDF_Standard_Exception == TPRes))
            {
                CHARPTR_WITH_LEN p_cpwl = default(CHARPTR_WITH_LEN);
                byte[] buf = null;
                buf = new byte[1001];

                GCHandle hBuf = default(GCHandle);
                hBuf = GCHandle.Alloc(buf, GCHandleType.Pinned);

                p_cpwl.charPtr = hBuf.AddrOfPinnedObject();
                p_cpwl.len = 1000;

                UInt32 innerTPRes = PDFArchiverHandle_GetLastExceptionText(m_hArchiver, TPRes, ref p_cpwl);
                hBuf.Free();

                if ((TOCRPDF_ErrorOK == innerTPRes))
                {
                    msg = System.Text.Encoding.ASCII.GetString(buf);
                }
                else if ((TOCRPDF_PoDoFo_Exception == innerTPRes))
                {
                    msg = "Unknown PoDoFo Exception";
                }
                else if ((TOCRPDF_Standard_Exception == innerTPRes))
                {
                    msg = "Unknown Standard Exception";
                }
                else
                {
                    msg = base.GetLastExceptionText(innerTPRes);
                }
            }
            else
            {
                msg = base.GetLastExceptionText(TPRes);
            }

            return msg;

        }

        public override UInt32 TocrPdfResult(UInt32 TPRes)
        {
            if ((TOCRPDF_ErrorOK != TPRes))
            {
                // ask the dll for the error msg - and later ask for the stack trace
                string msg = GetLastExceptionText(TPRes);

                Exception innerException = new Exception(msg);
                PDFExtractorException ex = new PDFExtractorException("TOCR PDF DLL is not able to do that. Error " + TPRes, innerException);
                throw (ex);
                //MsgBox("TOCR PDF DLL is not able to do that. Error " & TPRes, MsgBoxStyle.Exclamation)
            }

            return TPRes;
        }

        #region " IDisposable Support "
        // This method disposes the derived object's resources. 
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Insert code to free managed resources. 
                }
                // Insert code to free unmanaged resources. 
                TocrPdfResult(PDFArchiverHandle_Delete(m_hArchiver));
            }
            base.Dispose(disposing);
        }

        // The derived class does not have a Finalize method 
        // or a Dispose method with parameters because it inherits 
        // them from the base class. 
        #endregion

    }

    public class PDFArchiverBase : PDFBase
	{
        public override UInt32 TocrPdfResult(UInt32 TPRes)
		{
			if ((TOCRPDF_ErrorOK != TPRes)) {
				PDFArchiverException ex = new PDFArchiverException("TOCR PDF DLL is not able to do that. Error " + TPRes);
				throw (ex);
				//MsgBox("TOCR PDF DLL is not able to do that. Error " & TPRes, MsgBoxStyle.Exclamation)
			}

			return TPRes;
		}

		#region " IDisposable Support "
		// This method disposes the derived object's resources. 
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed) {
				if (disposing) {
					// Insert code to free managed resources. 
				}
				// Insert code to free unmanaged resources. 
			}
			base.Dispose(disposing);
		}

		// The derived class does not have a Finalize method 
		// or a Dispose method with parameters because it inherits 
		// them from the base class. 
		#endregion


	}

    public class PDFArchiverUser : PDFArchiverBase
    {
        public PDFArchiver m_pArchiver;
        protected PDFArchiverUser()
        {
            // don't call this as it would leave us with an unpopulated PDFExtractor
        }

        public PDFArchiverUser(PDFArchiver pArchiver)
        {
            m_pArchiver = pArchiver;
        }

        public override string GetLastExceptionText(UInt32 TPRes)
        {
            return m_pArchiver.GetLastExceptionText(TPRes);
        }

        public override UInt32 TocrPdfResult(UInt32 TPRes)
        {
            if ((TOCRPDF_ErrorOK != TPRes))
            {
                TPRes = m_pArchiver.TocrPdfResult(TPRes);
            }

            return TPRes;
        }


        #region " IDisposable Support "
        // This method disposes the derived object's resources. 
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Insert code to free managed resources. 
                }
                // Insert code to free unmanaged resources. 
            }
            base.Dispose(disposing);
        }

        // The derived class does not have a Finalize method 
        // or a Dispose method with parameters because it inherits 
        // them from the base class. 
        #endregion

    }

    public class PDFArchiverMemDoc : PDFArchiverUser
    {
        public PDFArchiverMemDocHandle m_hMemDoc;
        protected PDFArchiverMemDoc()
        {
            // don't call this as it would leave us with an unpopulated PDFArchiver
        }

        public PDFArchiverMemDoc(PDFArchiver pArchiver)
            : base(pArchiver)
        {
            TocrPdfResult(PDFArchiverMemDocHandle_New(ref m_hMemDoc));
        }

        public void Load(string inFileName)
        {
            GCHandle hInFileName = GCHandle.Alloc(inFileName, GCHandleType.Pinned);
            IntPtr pInFileName = hInFileName.AddrOfPinnedObject();
#if Win64
            Int64 iInFileName = pInFileName.ToInt64();
#else // not Win64
			Int32 iInFileName = pInFileName.ToInt32();
#endif // Win64

            TocrPdfResult(PDFArchiverHandle_Load(m_pArchiver.m_hArchiver, m_hMemDoc, iInFileName));
        }

        public void Close(string outFileName)
        {
            GCHandle hOutFileName = GCHandle.Alloc(outFileName, GCHandleType.Pinned);
            IntPtr pOutFileName = hOutFileName.AddrOfPinnedObject();
#if Win64
            Int64 iOutFileName = pOutFileName.ToInt64();
#else // not Win64
			Int32 iOutFileName = pOutFileName.ToInt32();
#endif // Win64

            TocrPdfResult(PDFArchiverHandle_Close(m_pArchiver.m_hArchiver, m_hMemDoc, iOutFileName));
        }

        public void SaveAppendix(string appendix, Int32 ImagePageNumber, TocrResultsInfo ri)
        {
            GCHandle hAppendix = GCHandle.Alloc(appendix, GCHandleType.Pinned);
            IntPtr pAppendix = hAppendix.AddrOfPinnedObject();
#if Win64
            Int64 iAppendix = pAppendix.ToInt64();
#else // not Win64
			Int32 iAppendix = pAppendix.ToInt32();
#endif // Win64

            TocrPdfResult(PDFArchiverHandle_SaveAppendix(m_pArchiver.m_hArchiver, m_hMemDoc, iAppendix, ImagePageNumber, ref ri));
        }
        
        public void SaveAllPdf(PDFArchiverMemDoc docIn, Int32 ImagePageNumber, TocrResultsInfo ri)
        {
            /*GCHandle hri = GCHandle.Alloc(ri, GCHandleType.Pinned);
            IntPtr pri = hri.AddrOfPinnedObject();
#if Win64
            Int64 iri = pri.ToInt64();
#else // not Win64
            Int32 iri = pri.ToInt32();
#endif // Win64
             */
            TocrPdfResult(PDFArchiverHandle_SaveAllPdf(m_pArchiver.m_hArchiver, m_hMemDoc, docIn.m_hMemDoc, ImagePageNumber, ref ri));
        }

        #region " IDisposable Support "
        // This method disposes the derived object's resources. 
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Insert code to free managed resources. 
                }
                // Insert code to free unmanaged resources. 
                TocrPdfResult(PDFArchiverMemDocHandle_Delete(m_hMemDoc));
            }
            base.Dispose(disposing);
        }

        // The derived class does not have a Finalize method 
        // or a Dispose method with parameters because it inherits 
        // them from the base class. 
        #endregion

    }

    public class PDFArchiverException : ApplicationException
    {

        public PDFArchiverException()
            : base()
        {
        }

        public PDFArchiverException(string message)
            : base(message)
        {
        }

        public PDFArchiverException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

    }

    #endregion
    #endregion
}
