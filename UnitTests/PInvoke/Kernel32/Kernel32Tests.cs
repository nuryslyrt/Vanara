﻿using NUnit.Framework;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Vanara.InteropServices;
using static Vanara.PInvoke.Kernel32;

namespace Vanara.PInvoke.Tests
{
	[TestFixture]
	public class Kernel32Tests
	{
		internal static readonly string fn = TestCaseSources.SmallFile;
		internal const string tmpstr = @"Temporary";

		public static string CreateTempFile(bool markAsTemp = true)
		{
			var fn = Path.GetTempFileName();
			if (markAsTemp)
				new FileInfo(fn).Attributes = FileAttributes.Temporary;
			File.WriteAllText(fn, tmpstr);
			return fn;
		}

		public static byte[] GetBigBytes(uint sz, byte fillVal = 0)
		{
			var ret = new byte[sz];
			for (var i = 0U; i < sz; i++) ret[i] = fillVal;
			return ret;
		}

		[Test]
		public void CeipIsOptedInTest()
		{
			Assert.That(() => CeipIsOptedIn(), Throws.Nothing);
		}

		[Test]
		public void CreateHardLinkTest()
		{
			var link = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			var fn = CreateTempFile();
			var b = CreateHardLink(link, fn);
			if (!b) TestContext.WriteLine($"CreateHardLink:{Win32Error.GetLastError()}");
			Assert.That(b);
			Assert.That(File.Exists(fn));
			var fnlen = new FileInfo(fn).Length;
			File.AppendAllText(link, "More text");
			Assert.That(fnlen, Is.LessThan(new FileInfo(fn).Length));
			File.Delete(link);
			File.Delete(fn);
		}

		[Test]
		public void CreateSymbolicLinkTest()
		{
			var link = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			var fn = CreateTempFile(false);
			Assert.That(File.Exists(fn));
			var b = CreateSymbolicLink(link, fn, SymbolicLinkType.SYMBOLIC_LINK_FLAG_FILE);
			if (!b) TestContext.WriteLine($"CreateSymbolicLink:{Win32Error.GetLastError()}");
			Assert.That(b);
			Assert.That(File.Exists(link));
			File.Delete(link);
			File.Delete(fn);
		}

		[Test]
		public void GetAppContainerNamedObjectPathTest()
		{
			var sb = new StringBuilder(1024);
			Assert.That(GetAppContainerNamedObjectPath(default, default, (uint)sb.Length, sb, out var len), ResultIs.Failure);
		}

		[Test]
		public void GetGamingDeviceModelInformationTest()
		{
			Assert.That(GetGamingDeviceModelInformation(out var i), Is.EqualTo((HRESULT)0));
			Assert.That(i.deviceId == GAMING_DEVICE_DEVICE_ID.GAMING_DEVICE_DEVICE_ID_NONE);
		}

		[Test]
		public void GlobalLockTest()
		{
			var bp = GlobalLock(new IntPtr(1));
			Assert.That(bp, Is.EqualTo(IntPtr.Zero));
			Assert.That(Marshal.GetLastWin32Error(), Is.Not.Zero);

			using (var hMem = SafeHGlobalHandle.CreateFromStructure(1L))
			{
				Assert.That(hMem.IsInvalid, Is.False);
				var ptr = GlobalLock(hMem.DangerousGetHandle());
				Assert.That(ptr, Is.EqualTo(hMem.DangerousGetHandle()));
				GlobalUnlock(hMem.DangerousGetHandle());
			}
		}

		[Test]
		public void QueryDosDeviceTest()
		{
			System.Collections.Generic.IEnumerable<string> ie = null;
			Assert.That(() => ie = QueryDosDevice("C:"), Throws.Nothing);
			Assert.That(ie, Is.Not.Null);
			TestContext.WriteLine(string.Join(",", ie));
			Assert.That(ie, Is.Not.Empty);
		}

		[Test]
		public void QueryDosDeviceTest1()
		{
			var sb = new StringBuilder(4096);
			var cch = QueryDosDevice("C:", sb, sb.Capacity);
			Assert.That(cch, Is.Not.Zero);
			Assert.That(sb.Length, Is.GreaterThan(0));
			TestContext.WriteLine(sb);
		}

		[Test]
		public void SetLastErrorTest()
		{
			SetLastError(0);
			Assert.That((uint)Win32Error.GetLastError(), Is.EqualTo(0U));
			SetLastError(Win32Error.ERROR_AUDIT_FAILED);
			Assert.That((uint)Win32Error.GetLastError(), Is.EqualTo(Win32Error.ERROR_AUDIT_FAILED));
		}
	}
}