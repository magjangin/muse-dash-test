using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppSystem;

namespace Il2Cpp;

[ObfuscatedName("<PrivateImplementationDetails>")]
public sealed class _PrivateImplementationDetails_ : Il2CppSystem.Object
{
	[StructLayout(LayoutKind.Explicit)]
	[ObfuscatedName("<PrivateImplementationDetails>+__StaticArrayInitTypeSize=20")]
	public struct ValueTypeNPrivateSealed0
	{
		static ValueTypeNPrivateSealed0()
		{
			Il2CppClassPointerStore<ValueTypeNPrivateSealed0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<_PrivateImplementationDetails_>.NativeClassPtr, "__StaticArrayInitTypeSize=20");
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ValueTypeNPrivateSealed0>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	[ObfuscatedName("<PrivateImplementationDetails>+__StaticArrayInitTypeSize=24")]
	public struct ValueTypeNPrivateSealed1
	{
		static ValueTypeNPrivateSealed1()
		{
			Il2CppClassPointerStore<ValueTypeNPrivateSealed1>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<_PrivateImplementationDetails_>.NativeClassPtr, "__StaticArrayInitTypeSize=24");
		}

		public unsafe Il2CppSystem.Object BoxIl2CppObject()
		{
			return new Il2CppSystem.Object(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<ValueTypeNPrivateSealed1>.NativeClassPtr, (System.IntPtr)(nint)Unsafe.AsPointer(ref this)));
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__6D78A7DEB7B1A2A73F2CDFA8EFC4FE6DDCC4E47A;

	private static readonly System.IntPtr NativeFieldInfoPtr_B6E6EA57C32297E83203480EE50A22C3581AA09C;

	public unsafe static ValueTypeNPrivateSealed1 _6D78A7DEB7B1A2A73F2CDFA8EFC4FE6DDCC4E47A
	{
		get
		{
			Unsafe.SkipInit(out ValueTypeNPrivateSealed1 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr__6D78A7DEB7B1A2A73F2CDFA8EFC4FE6DDCC4E47A, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr__6D78A7DEB7B1A2A73F2CDFA8EFC4FE6DDCC4E47A, (void*)(&valueTypeNPrivateSealed));
		}
	}

	public unsafe static ValueTypeNPrivateSealed0 B6E6EA57C32297E83203480EE50A22C3581AA09C
	{
		get
		{
			Unsafe.SkipInit(out ValueTypeNPrivateSealed0 result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_B6E6EA57C32297E83203480EE50A22C3581AA09C, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_B6E6EA57C32297E83203480EE50A22C3581AA09C, (void*)(&valueTypeNPrivateSealed));
		}
	}

	static _PrivateImplementationDetails_()
	{
		Il2CppClassPointerStore<_PrivateImplementationDetails_>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "", "<PrivateImplementationDetails>");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<_PrivateImplementationDetails_>.NativeClassPtr);
		NativeFieldInfoPtr__6D78A7DEB7B1A2A73F2CDFA8EFC4FE6DDCC4E47A = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_PrivateImplementationDetails_>.NativeClassPtr, "6D78A7DEB7B1A2A73F2CDFA8EFC4FE6DDCC4E47A");
		NativeFieldInfoPtr_B6E6EA57C32297E83203480EE50A22C3581AA09C = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_PrivateImplementationDetails_>.NativeClassPtr, "B6E6EA57C32297E83203480EE50A22C3581AA09C");
	}

	public _PrivateImplementationDetails_(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
