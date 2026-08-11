using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;

namespace Il2CppSpine;

public static class SpineSkeletonExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_IsWeighted_Public_Static_Boolean_VertexAttachment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InheritsRotation_Public_Static_Boolean_TransformMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InheritsScale_Public_Static_Boolean_TransformMode_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetKeyedItemsToSetupPose_Public_Static_Void_Animation_Skeleton_0;

	static SpineSkeletonExtensions()
	{
		Il2CppClassPointerStore<SpineSkeletonExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "SpineSkeletonExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpineSkeletonExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_IsWeighted_Public_Static_Boolean_VertexAttachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSkeletonExtensions>.NativeClassPtr, 100663846);
		NativeMethodInfoPtr_InheritsRotation_Public_Static_Boolean_TransformMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSkeletonExtensions>.NativeClassPtr, 100663847);
		NativeMethodInfoPtr_InheritsScale_Public_Static_Boolean_TransformMode_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSkeletonExtensions>.NativeClassPtr, 100663848);
		NativeMethodInfoPtr_SetKeyedItemsToSetupPose_Public_Static_Void_Animation_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSkeletonExtensions>.NativeClassPtr, 100663849);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 788211, RefRangeEnd = 788214, XrefRangeStart = 788211, XrefRangeEnd = 788211, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool IsWeighted(this VertexAttachment va)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)va);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsWeighted_Public_Static_Boolean_VertexAttachment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 721961, RefRangeEnd = 721964, XrefRangeStart = 721961, XrefRangeEnd = 721964, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool InheritsRotation(this TransformMode mode)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&mode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InheritsRotation_Public_Static_Boolean_TransformMode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788214, RefRangeEnd = 788215, XrefRangeStart = 788214, XrefRangeEnd = 788214, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static bool InheritsScale(this TransformMode mode)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&mode);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InheritsScale_Public_Static_Boolean_TransformMode_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 788216, RefRangeEnd = 788218, XrefRangeStart = 788215, XrefRangeEnd = 788216, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void SetKeyedItemsToSetupPose(this Animation animation, Skeleton skeleton)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetKeyedItemsToSetupPose_Public_Static_Void_Animation_Skeleton_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SpineSkeletonExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
