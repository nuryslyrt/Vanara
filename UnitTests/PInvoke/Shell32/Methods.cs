﻿using NUnit.Framework;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using static Vanara.PInvoke.Shell32;

namespace Vanara.PInvoke.Tests
{
	[TestFixture]
	public class Shell32Methods
	{
		[Test]
		public void CommandLineToArgvWTest()
		{
			const string cmd = "-d -s \"Str\" --hog";
			var h = CommandLineToArgvW(cmd, out var c);
			Assert.That(h.IsInvalid, Is.False);
			Assert.That(c, Is.EqualTo(4));
			Assert.That(h.ToStringEnum(c, System.Runtime.InteropServices.CharSet.Unicode), Is.Not.Empty);
			Assert.That(h.ToStringEnum(c, System.Runtime.InteropServices.CharSet.Unicode).First(), Is.EqualTo("-d"));

			Assert.That(CommandLineToArgvW(cmd).First(), Is.EqualTo("-d"));
			TestContext.WriteLine(string.Join(" | ", CommandLineToArgvW(cmd)));
		}

		[Test]
		public void AssocCreateForClassesTest()
		{
			Assert.Fail("Not implemented.");
			//Assert.That(AssocCreateForClasses(), Is.Zero);
		}

		[Test]
		public void AssocGetDetailsOfPropKeyTest()
		{
			Assert.Fail("Not implemented.");
			//Assert.That(AssocGetDetailsOfPropKey(), Is.Zero);
		}

		[Test]
		public void SHGetDesktopFolderTest()
		{
			Assert.That(SHGetDesktopFolder(out var sf), ResultIs.Successful);
			var eo = sf.EnumObjects(HWND.NULL, SHCONTF.SHCONTF_NONFOLDERS);
			Assert.That(eo, Is.Not.Null);
			foreach (var sub in new Collections.IEnumFromNext<IntPtr>((out IntPtr p) => eo.Next(1, out p, out var f).Succeeded && f == 1, () => { try { eo.Reset(); } catch { } }))
			{
				STRRET name = default;
				Assert.That(() => sf.GetDisplayNameOf(sub, SHGDNF.SHGDN_NORMAL | SHGDNF.SHGDN_INFOLDER, out name), Throws.Nothing);
				TestContext.WriteLine(name);
			}
			Marshal.ReleaseComObject(eo);
			Marshal.ReleaseComObject(sf);
		}

		/*
		AssocCreateForClasses
		AssocGetDetailsOfPropKey
		CDefFolderMenu_Create2
		DragAcceptFiles
		DragFinish
		DragQueryFile
		DragQueryPoint
		DuplicateIcon
		ExtractAssociatedIcon
		ExtractAssociatedIconEx
		ExtractIcon
		ExtractIconEx
		FindExecutable
		GetCurrentProcessExplicitAppUserModelID
		GetFileNameFromBrowse
		InitNetworkAddressControl
		IsNetDrive
		IsUserAnAdmin
		OpenRegStream
		PathCleanupSpec
		PathGetShortPath
		PathIsExe
		PathIsSlow
		PathMakeUniqueName
		PathResolve
		PathYetAnotherMakeUniqueName
		PickIconDlg
		PifMgr_CloseProperties
		PifMgr_GetProperties
		PifMgr_OpenProperties
		PifMgr_SetProperties
		ReadCabinetState
		RealDriveType
		RestartDialog
		RestartDialogEx
		SetCurrentProcessExplicitAppUserModelID
		SHAddDefaultPropertiesByExt
		SHAddFromPropSheetExtArray
		SHAddToRecentDocs
		SHAlloc
		SHAppBarMessage
		SHAssocEnumHandlers
		SHAssocEnumHandlersForProtocolByApplication
		SHBindToFolderIDListParent
		SHBindToFolderIDListParentEx
		SHBindToObject
		SHBindToParent
		SHBrowseForFolder
		SHChangeNotification_Lock
		SHChangeNotification_Unlock
		SHChangeNotify
		SHChangeNotifyDeregister
		SHChangeNotifyRegister
		SHChangeNotifyRegisterThread
		SHCreateAssociationRegistration
		SHCreateDataObject
		SHCreateDefaultContextMenu
		SHCreateDefaultExtractIcon
		SHCreateDefaultPropertiesOp
		SHCreateDirectory
		SHCreateDirectoryEx
		SHCreateFileExtractIconW
		SHCreateItemFromIDList
		SHCreateItemFromParsingName
		SHCreateItemFromRelativeName
		SHCreateItemInKnownFolder
		SHCreateItemWithParent
		SHCreatePropSheetExtArray
		SHCreateShellFolderView
		SHCreateShellFolderViewEx
		SHCreateShellItem
		SHCreateShellItemArray
		SHCreateShellItemArrayFromDataObject
		SHCreateShellItemArrayFromIDLists
		SHCreateShellItemArrayFromShellItem
		SHCreateStdEnumFmtEtc
		SHDefExtractIcon
		SHDestroyPropSheetExtArray
		SHDoDragDrop
		Shell_GetCachedImageIndex
		Shell_GetImageLists
		Shell_MergeMenus
		Shell_NotifyIcon
		Shell_NotifyIconGetRect
		ShellAbout
		ShellExecute
		ShellExecuteEx
		SHEmptyRecycleBin
		SHEnumerateUnreadMailAccountsA
		SHEnumerateUnreadMailAccountsW
		SHEvaluateSystemCommandTemplate
		SHFileOperation
		SHFind_InitMenuPopup
		SHFindFiles
		SHFlushSFCache
		SHFormatDrive
		SHFree
		SHFreeNameMappings
		SHGetAttributesFromDataObject
		SHGetDataFromIDList
		SHGetDiskFreeSpaceA
		SHGetDiskFreeSpaceEx
		SHGetDiskFreeSpaceW
		SHGetDriveMedia
		SHGetFileInfo
		SHGetFolderLocation
		SHGetFolderPath
		SHGetFolderPathAndSubDir
		SHGetFolderPathEx
		SHGetIconOverlayIndex
		SHGetIDListFromObject
		SHGetImageList
		SHGetInstanceExplorer
		SHGetItemFromDataObject
		SHGetItemFromObject
		SHGetKnownFolderIDList
		SHGetKnownFolderItem
		SHGetKnownFolderPath
		SHGetLocalizedName
		SHGetNameFromIDList
		SHGetNewLinkInfo
		SHGetPathFromIDList
		SHGetPathFromIDListEx
		SHGetPropertyStoreForWindow
		SHGetPropertyStoreFromIDList
		SHGetPropertyStoreFromParsingName
		SHGetRealIDL
		SHGetSetFolderCustomSettings
		SHGetSetSettings
		SHGetSettings
		SHGetStockIconInfo
		SHGetTemporaryPropertyForItem
		SHGetUnreadMailCountW
		SHHandleUpdateImage
		SHInvokePrinterCommand
		SHIsFileAvailableOffline
		SHLimitInputEdit
		SHLoadInProc
		SHLoadNonloadedIconOverlayIdentifiers
		SHMapPIDLToSystemImageListIndex
		SHMultiFileProperties
		SHObjectProperties
		SHOpenFolderAndSelectItems
		SHOpenWithDialog
		SHParseDisplayName
		SHPathPrepareForWrite
		SHPropStgCreate
		SHPropStgReadMultiple
		SHPropStgWriteMultiple
		SHQueryRecycleBin
		SHQueryUserNotificationState
		SHRemoveLocalizedName
		SHReplaceFromPropSheetExtArray
		SHResolveLibrary
		SHRestricted
		SHSetDefaultProperties
		SHSetInstanceExplorer
		SHSetKnownFolderPath
		SHSetLocalizedName
		SHSetTemporaryPropertyForItem
		SHSetUnreadMailCountW
		SHShellFolderView_Message
		SHShowManageLibraryUI
		SHSimpleIDListFromPath
		SHTestTokenMembership
		SHUpdateImage
		SHValidateUNC
		SignalFileOpen
		StgMakeUniqueName
		Win32DeleteFile
		WriteCabinetState
		*/
	}
}
