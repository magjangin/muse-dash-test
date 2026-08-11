using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine.Unity.AttachmentTools;

public static class AttachmentRegionExtensions : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeMethodInfoPtr_GetRegion_Public_Static_AtlasRegion_Attachment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetRegion_Public_Static_Void_Attachment_AtlasRegion_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetRegion_Public_Static_Void_RegionAttachment_AtlasRegion_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetRegion_Public_Static_Void_MeshAttachment_AtlasRegion_Boolean_0;

	static AttachmentRegionExtensions()
	{
		Il2CppClassPointerStore<AttachmentRegionExtensions>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity.AttachmentTools", "AttachmentRegionExtensions");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AttachmentRegionExtensions>.NativeClassPtr);
		NativeMethodInfoPtr_GetRegion_Public_Static_AtlasRegion_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentRegionExtensions>.NativeClassPtr, 100664441);
		NativeMethodInfoPtr_SetRegion_Public_Static_Void_Attachment_AtlasRegion_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentRegionExtensions>.NativeClassPtr, 100664442);
		NativeMethodInfoPtr_SetRegion_Public_Static_Void_RegionAttachment_AtlasRegion_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentRegionExtensions>.NativeClassPtr, 100664443);
		NativeMethodInfoPtr_SetRegion_Public_Static_Void_MeshAttachment_AtlasRegion_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentRegionExtensions>.NativeClassPtr, 100664444);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 794256, RefRangeEnd = 794258, XrefRangeStart = 794251, XrefRangeEnd = 794256, MetadataInitTokenRva = 47239200L, MetadataInitFlagRva = 59827123L)]
	public unsafe static AtlasRegion GetRegion(this Attachment attachment)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetRegion_Public_Static_AtlasRegion_Attachment_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 794270, RefRangeEnd = 794272, XrefRangeStart = 794258, XrefRangeEnd = 794270, MetadataInitTokenRva = 47239224L, MetadataInitFlagRva = 59827124L)]
	public unsafe static void SetRegion(this Attachment attachment, AtlasRegion region, bool updateOffset = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)region);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &updateOffset;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRegion_Public_Static_Void_Attachment_AtlasRegion_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794272, XrefRangeEnd = 794276, MetadataInitTokenRva = 47239288L, MetadataInitFlagRva = 59827125L)]
	public unsafe static void SetRegion(this RegionAttachment attachment, AtlasRegion region, bool updateOffset = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)region);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &updateOffset;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRegion_Public_Static_Void_RegionAttachment_AtlasRegion_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794276, XrefRangeEnd = 794279, MetadataInitTokenRva = 47239300L, MetadataInitFlagRva = 59827126L)]
	public unsafe static void SetRegion(this MeshAttachment attachment, AtlasRegion region, bool updateUVs = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)region);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &updateUVs;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRegion_Public_Static_Void_MeshAttachment_AtlasRegion_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AttachmentRegionExtensions(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
