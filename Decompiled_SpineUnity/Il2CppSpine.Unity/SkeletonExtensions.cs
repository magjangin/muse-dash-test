using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppSpine.Unity;

public static class SkeletonExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_SetColor_Public_Static_Void_Skeleton_Color_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetWorldPosition_Public_Static_Vector3_Bone_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetQuaternion_Public_Static_Quaternion_Bone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetWorldToLocalMatrix_Public_Static_Void_Bone_byref_Single_byref_Single_byref_Single_byref_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLocalVertices_Public_Static_Il2CppStructArray_1_Vector2_VertexAttachment_Slot_Il2CppStructArray_1_Vector2_0;

	static SkeletonExtensions()
	{
		Il2CppClassPointerStore<SkeletonExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_SetColor_Public_Static_Void_Skeleton_Color_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonExtensions>.NativeClassPtr, 100664396);
		NativeMethodInfoPtr_GetWorldPosition_Public_Static_Vector3_Bone_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonExtensions>.NativeClassPtr, 100664397);
		NativeMethodInfoPtr_GetQuaternion_Public_Static_Quaternion_Bone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonExtensions>.NativeClassPtr, 100664398);
		NativeMethodInfoPtr_GetWorldToLocalMatrix_Public_Static_Void_Bone_byref_Single_byref_Single_byref_Single_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonExtensions>.NativeClassPtr, 100664399);
		NativeMethodInfoPtr_GetLocalVertices_Public_Static_Il2CppStructArray_1_Vector2_VertexAttachment_Slot_Il2CppStructArray_1_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonExtensions>.NativeClassPtr, 100664400);
	}

	[CallerCount(0)]
	public unsafe static void SetColor(this Skeleton skeleton, Color color)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(Color**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &color;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetColor_Public_Static_Void_Skeleton_Color_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 793432, RefRangeEnd = 793439, XrefRangeStart = 793430, XrefRangeEnd = 793432, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Vector3 GetWorldPosition(this Bone bone, Transform spineGameObjectTransform)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spineGameObjectTransform);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetWorldPosition_Public_Static_Vector3_Bone_Transform_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector3*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793439, XrefRangeEnd = 793446, MetadataInitTokenRva = 46270788L, MetadataInitFlagRva = 59827230L)]
	public unsafe static Quaternion GetQuaternion(this Bone bone)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetQuaternion_Public_Static_Quaternion_Bone_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Quaternion*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe static void GetWorldToLocalMatrix(this Bone bone, out float ia, out float ib, out float ic, out float id)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref ia);
		*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref ib);
		*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref ic);
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref id);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetWorldToLocalMatrix_Public_Static_Void_Bone_byref_Single_byref_Single_byref_Single_byref_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 793453, RefRangeEnd = 793456, XrefRangeStart = 793446, XrefRangeEnd = 793453, MetadataInitTokenRva = 46270756L, MetadataInitFlagRva = 59827231L)]
	public unsafe static Il2CppStructArray<Vector2> GetLocalVertices(this VertexAttachment va, Slot slot, Il2CppStructArray<Vector2> buffer)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)va);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)buffer);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLocalVertices_Public_Static_Il2CppStructArray_1_Vector2_VertexAttachment_Slot_Il2CppStructArray_1_Vector2_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<Vector2>>(intPtr) : null;
	}

	public SkeletonExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
