using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class AtlasAttachmentLoader : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_atlasArray;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_Atlas_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewRegionAttachment_Public_Virtual_Final_New_RegionAttachment_Skin_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewMeshAttachment_Public_Virtual_Final_New_MeshAttachment_Skin_String_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewBoundingBoxAttachment_Public_Virtual_Final_New_BoundingBoxAttachment_Skin_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewPathAttachment_Public_Virtual_Final_New_PathAttachment_Skin_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewPointAttachment_Public_Virtual_Final_New_PointAttachment_Skin_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewClippingAttachment_Public_Virtual_Final_New_ClippingAttachment_Skin_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindRegion_Public_AtlasRegion_String_0;

	public unsafe Il2CppReferenceArray<Atlas> atlasArray
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlasArray);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Atlas>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlasArray)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	static AtlasAttachmentLoader()
	{
		Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "AtlasAttachmentLoader");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr);
		NativeFieldInfoPtr_atlasArray = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr, "atlasArray");
		NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_Atlas_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr, 100663510);
		NativeMethodInfoPtr_NewRegionAttachment_Public_Virtual_Final_New_RegionAttachment_Skin_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr, 100663511);
		NativeMethodInfoPtr_NewMeshAttachment_Public_Virtual_Final_New_MeshAttachment_Skin_String_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr, 100663512);
		NativeMethodInfoPtr_NewBoundingBoxAttachment_Public_Virtual_Final_New_BoundingBoxAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr, 100663513);
		NativeMethodInfoPtr_NewPathAttachment_Public_Virtual_Final_New_PathAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr, 100663514);
		NativeMethodInfoPtr_NewPointAttachment_Public_Virtual_Final_New_PointAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr, 100663515);
		NativeMethodInfoPtr_NewClippingAttachment_Public_Virtual_Final_New_ClippingAttachment_Skin_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr, 100663516);
		NativeMethodInfoPtr_FindRegion_Public_AtlasRegion_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr, 100663517);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783336, RefRangeEnd = 783337, XrefRangeStart = 783327, XrefRangeEnd = 783336, MetadataInitTokenRva = 47237204L, MetadataInitFlagRva = 59849637L)]
	public unsafe AtlasAttachmentLoader([Optional] Il2CppReferenceArray<Atlas> atlasArray)
	{
		if (atlasArray == null)
		{
			atlasArray = new Il2CppReferenceArray<Atlas>(0L);
		}
		this._002Ector(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AtlasAttachmentLoader>.NativeClassPtr));
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlasArray);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Il2CppReferenceArray_1_Atlas_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783337, XrefRangeEnd = 783344, MetadataInitTokenRva = 47237172L, MetadataInitFlagRva = 59849638L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783344, XrefRangeEnd = 783350, MetadataInitTokenRva = 47237044L, MetadataInitFlagRva = 59849639L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783350, XrefRangeEnd = 783357, MetadataInitTokenRva = 47236972L, MetadataInitFlagRva = 59849640L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783357, XrefRangeEnd = 783364, MetadataInitTokenRva = 47237072L, MetadataInitFlagRva = 59849641L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783364, XrefRangeEnd = 783370, MetadataInitTokenRva = 47237108L, MetadataInitFlagRva = 59849642L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783370, XrefRangeEnd = 783377, MetadataInitTokenRva = 47237004L, MetadataInitFlagRva = 59849643L)]
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

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 783384, RefRangeEnd = 783386, XrefRangeStart = 783377, XrefRangeEnd = 783384, MetadataInitTokenRva = 47238740L, MetadataInitFlagRva = 59849636L)]
	public unsafe AtlasRegion FindRegion(string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindRegion_Public_AtlasRegion_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
	}

	public AtlasAttachmentLoader(params Atlas[] atlasArray)
		: this(new Il2CppReferenceArray<Atlas>(atlasArray))
	{
	}

	public AtlasAttachmentLoader(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
