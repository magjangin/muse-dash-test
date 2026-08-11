using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public static class MathUtils : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_random;

	private static readonly System.IntPtr NativeMethodInfoPtr_Sin_Public_Static_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Cos_Public_Static_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SinDeg_Public_Static_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CosDeg_Public_Static_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Atan2_Public_Static_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clamp_Public_Static_Single_Single_Single_Single_0;

	public unsafe static Il2CppSystem.Random random
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_random, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Random>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_random, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)random));
		}
	}

	static MathUtils()
	{
		Il2CppClassPointerStore<MathUtils>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "MathUtils");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MathUtils>.NativeClassPtr);
		NativeFieldInfoPtr_random = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MathUtils>.NativeClassPtr, "random");
		NativeMethodInfoPtr_Sin_Public_Static_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MathUtils>.NativeClassPtr, 100663689);
		NativeMethodInfoPtr_Cos_Public_Static_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MathUtils>.NativeClassPtr, 100663690);
		NativeMethodInfoPtr_SinDeg_Public_Static_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MathUtils>.NativeClassPtr, 100663691);
		NativeMethodInfoPtr_CosDeg_Public_Static_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MathUtils>.NativeClassPtr, 100663692);
		NativeMethodInfoPtr_Atan2_Public_Static_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MathUtils>.NativeClassPtr, 100663693);
		NativeMethodInfoPtr_Clamp_Public_Static_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MathUtils>.NativeClassPtr, 100663694);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 784234, RefRangeEnd = 784240, XrefRangeStart = 784230, XrefRangeEnd = 784234, MetadataInitTokenRva = 46341276L, MetadataInitFlagRva = 59849684L)]
	public unsafe static float Sin(float radians)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&radians);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Sin_Public_Static_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 784244, RefRangeEnd = 784250, XrefRangeStart = 784240, XrefRangeEnd = 784244, MetadataInitTokenRva = 46340840L, MetadataInitFlagRva = 59849685L)]
	public unsafe static float Cos(float radians)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&radians);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Cos_Public_Static_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 784254, RefRangeEnd = 784255, XrefRangeStart = 784250, XrefRangeEnd = 784254, MetadataInitTokenRva = 46341212L, MetadataInitFlagRva = 59849686L)]
	public unsafe static float SinDeg(float degrees)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&degrees);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SinDeg_Public_Static_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 784259, RefRangeEnd = 784261, XrefRangeStart = 784255, XrefRangeEnd = 784259, MetadataInitTokenRva = 46340780L, MetadataInitFlagRva = 59849687L)]
	public unsafe static float CosDeg(float degrees)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&degrees);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CosDeg_Public_Static_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(17)]
	[CachedScanResults(RefRangeStart = 784265, RefRangeEnd = 784282, XrefRangeStart = 784261, XrefRangeEnd = 784265, MetadataInitTokenRva = 46340612L, MetadataInitFlagRva = 59849688L)]
	public unsafe static float Atan2(float y, float x)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&y);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &x;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Atan2_Public_Static_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(255)]
	[CachedScanResults(RefRangeStart = 722059, RefRangeEnd = 722314, XrefRangeStart = 722059, XrefRangeEnd = 722314, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static float Clamp(float value, float min, float max)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&value);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &min;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &max;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Clamp_Public_Static_Single_Single_Single_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public MathUtils(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
