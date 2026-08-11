using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine.Unity;

public class RegionlessAttachmentLoader : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_emptyRegion;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_EmptyRegion_Private_Static_get_AtlasRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewRegionAttachment_Public_Virtual_Final_New_RegionAttachment_Skin_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewMeshAttachment_Public_Virtual_Final_New_MeshAttachment_Skin_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewBoundingBoxAttachment_Public_Virtual_Final_New_BoundingBoxAttachment_Skin_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewPathAttachment_Public_Virtual_Final_New_PathAttachment_Skin_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewPointAttachment_Public_Virtual_Final_New_PointAttachment_Skin_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewClippingAttachment_Public_Virtual_Final_New_ClippingAttachment_Skin_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static AtlasRegion emptyRegion
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_emptyRegion, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_emptyRegion, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlasRegion));
		}
	}

	public unsafe static AtlasRegion EmptyRegion
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 788286, RefRangeEnd = 788288, XrefRangeStart = 788262, XrefRangeEnd = 788286, MetadataInitTokenRva = 47123384L, MetadataInitFlagRva = 59827202L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EmptyRegion_Private_Static_get_AtlasRegion_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
		}
	}

	static RegionlessAttachmentLoader()
	{
		Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "RegionlessAttachmentLoader");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr);
		NativeFieldInfoPtr_emptyRegion = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr, "emptyRegion");
		NativeMethodInfoPtr_get_EmptyRegion_Private_Static_get_AtlasRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr, 100663866);
		NativeMethodInfoPtr_NewRegionAttachment_Public_Virtual_Final_New_RegionAttachment_Skin_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr, 100663867);
		NativeMethodInfoPtr_NewMeshAttachment_Public_Virtual_Final_New_MeshAttachment_Skin_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr, 100663868);
		NativeMethodInfoPtr_NewBoundingBoxAttachment_Public_Virtual_Final_New_BoundingBoxAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr, 100663869);
		NativeMethodInfoPtr_NewPathAttachment_Public_Virtual_Final_New_PathAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr, 100663870);
		NativeMethodInfoPtr_NewPointAttachment_Public_Virtual_Final_New_PointAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr, 100663871);
		NativeMethodInfoPtr_NewClippingAttachment_Public_Virtual_Final_New_ClippingAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr, 100663872);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr, 100663873);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788288, XrefRangeEnd = 788294, MetadataInitTokenRva = 47123328L, MetadataInitFlagRva = 59827203L)]
	public unsafe virtual RegionAttachment NewRegionAttachment(Skin skin, string name, string path)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(path);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewRegionAttachment_Public_Virtual_Final_New_RegionAttachment_Skin_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RegionAttachment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788294, XrefRangeEnd = 788300, MetadataInitTokenRva = 47123216L, MetadataInitFlagRva = 59827204L)]
	public unsafe virtual MeshAttachment NewMeshAttachment(Skin skin, string name, string path)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(path);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewMeshAttachment_Public_Virtual_Final_New_MeshAttachment_Skin_String_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MeshAttachment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788300, XrefRangeEnd = 788304, MetadataInitTokenRva = 47123136L, MetadataInitFlagRva = 59827205L)]
	public unsafe virtual BoundingBoxAttachment NewBoundingBoxAttachment(Skin skin, string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewBoundingBoxAttachment_Public_Virtual_Final_New_BoundingBoxAttachment_Skin_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<BoundingBoxAttachment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788304, XrefRangeEnd = 788308, MetadataInitTokenRva = 47123252L, MetadataInitFlagRva = 59827206L)]
	public unsafe virtual PathAttachment NewPathAttachment(Skin skin, string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewPathAttachment_Public_Virtual_Final_New_PathAttachment_Skin_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PathAttachment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788308, XrefRangeEnd = 788312, MetadataInitTokenRva = 47123316L, MetadataInitFlagRva = 59827207L)]
	public unsafe virtual PointAttachment NewPointAttachment(Skin skin, string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewPointAttachment_Public_Virtual_Final_New_PointAttachment_Skin_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PointAttachment>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788312, XrefRangeEnd = 788316, MetadataInitTokenRva = 47123196L, MetadataInitFlagRva = 59827208L)]
	public unsafe virtual ClippingAttachment NewClippingAttachment(Skin skin, string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewClippingAttachment_Public_Virtual_Final_New_ClippingAttachment_Skin_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ClippingAttachment>(intPtr) : null;
	}

	[CallerCount(2392)]
	[CachedScanResults(RefRangeStart = 18875, RefRangeEnd = 21267, XrefRangeStart = 18875, XrefRangeEnd = 21267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RegionlessAttachmentLoader()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RegionlessAttachmentLoader>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RegionlessAttachmentLoader(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
