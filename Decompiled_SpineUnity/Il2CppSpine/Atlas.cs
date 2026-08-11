using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.IO;

namespace Il2CppSpine;

public class Atlas : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_pages;

	private static readonly System.IntPtr NativeFieldInfoPtr_regions;

	private static readonly System.IntPtr NativeFieldInfoPtr_textureLoader;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetEnumerator_Public_Virtual_Final_New_IEnumerator_1_AtlasRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_TextReader_String_TextureLoader_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_List_1_AtlasPage_List_1_AtlasRegion_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Load_Private_Void_TextReader_String_TextureLoader_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadValue_Private_Static_String_TextReader_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReadTuple_Private_Static_Int32_TextReader_Il2CppStringArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FlipV_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindRegion_Public_AtlasRegion_String_0;

	public unsafe List<AtlasPage> pages
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pages);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<AtlasPage>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pages)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<AtlasRegion> regions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_regions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<AtlasRegion>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_regions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe TextureLoader textureLoader
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_textureLoader);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TextureLoader>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_textureLoader)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textureLoader));
		}
	}

	static Atlas()
	{
		Il2CppClassPointerStore<Atlas>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "Atlas");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Atlas>.NativeClassPtr);
		NativeFieldInfoPtr_pages = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Atlas>.NativeClassPtr, "pages");
		NativeFieldInfoPtr_regions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Atlas>.NativeClassPtr, "regions");
		NativeFieldInfoPtr_textureLoader = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Atlas>.NativeClassPtr, "textureLoader");
		NativeMethodInfoPtr_GetEnumerator_Public_Virtual_Final_New_IEnumerator_1_AtlasRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Atlas>.NativeClassPtr, 100663496);
		NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Atlas>.NativeClassPtr, 100663497);
		NativeMethodInfoPtr__ctor_Public_Void_TextReader_String_TextureLoader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Atlas>.NativeClassPtr, 100663498);
		NativeMethodInfoPtr__ctor_Public_Void_List_1_AtlasPage_List_1_AtlasRegion_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Atlas>.NativeClassPtr, 100663499);
		NativeMethodInfoPtr_Load_Private_Void_TextReader_String_TextureLoader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Atlas>.NativeClassPtr, 100663500);
		NativeMethodInfoPtr_ReadValue_Private_Static_String_TextReader_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Atlas>.NativeClassPtr, 100663501);
		NativeMethodInfoPtr_ReadTuple_Private_Static_Int32_TextReader_Il2CppStringArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Atlas>.NativeClassPtr, 100663502);
		NativeMethodInfoPtr_FlipV_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Atlas>.NativeClassPtr, 100663503);
		NativeMethodInfoPtr_FindRegion_Public_AtlasRegion_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Atlas>.NativeClassPtr, 100663504);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783113, RefRangeEnd = 783114, XrefRangeStart = 783108, XrefRangeEnd = 783113, MetadataInitTokenRva = 47238848L, MetadataInitFlagRva = 59849628L)]
	public unsafe virtual IEnumerator<AtlasRegion> GetEnumerator()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetEnumerator_Public_Virtual_Final_New_IEnumerator_1_AtlasRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator<AtlasRegion>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783114, XrefRangeEnd = 783119, MetadataInitTokenRva = 47238984L, MetadataInitFlagRva = 59849629L)]
	public unsafe virtual IEnumerator System_Collections_IEnumerable_GetEnumerator()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerable_GetEnumerator_Private_Virtual_Final_New_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783132, RefRangeEnd = 783133, XrefRangeStart = 783119, XrefRangeEnd = 783132, MetadataInitTokenRva = 47239088L, MetadataInitFlagRva = 59849630L)]
	public unsafe Atlas(TextReader reader, string dir, TextureLoader textureLoader)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Atlas>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(dir);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textureLoader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_TextReader_String_TextureLoader_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783148, RefRangeEnd = 783149, XrefRangeStart = 783133, XrefRangeEnd = 783148, MetadataInitTokenRva = 47239044L, MetadataInitFlagRva = 59849631L)]
	public unsafe Atlas(List<AtlasPage> pages, List<AtlasRegion> regions)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Atlas>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pages);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)regions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_List_1_AtlasPage_List_1_AtlasRegion_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783278, RefRangeEnd = 783279, XrefRangeStart = 783149, XrefRangeEnd = 783278, MetadataInitTokenRva = 47238856L, MetadataInitFlagRva = 59849632L)]
	public unsafe void Load(TextReader reader, string imagesDir, TextureLoader textureLoader)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(imagesDir);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textureLoader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Load_Private_Void_TextReader_String_TextureLoader_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 783291, RefRangeEnd = 783294, XrefRangeStart = 783279, XrefRangeEnd = 783291, MetadataInitTokenRva = 47238960L, MetadataInitFlagRva = 59849633L)]
	public unsafe static string ReadValue(TextReader reader)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadValue_Private_Static_String_TextReader_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 783305, RefRangeEnd = 783314, XrefRangeStart = 783294, XrefRangeEnd = 783305, MetadataInitTokenRva = 47238916L, MetadataInitFlagRva = 59849634L)]
	public unsafe static int ReadTuple(TextReader reader, Il2CppStringArray tuple)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)reader);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tuple);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReadTuple_Private_Static_Int32_TextReader_Il2CppStringArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783316, RefRangeEnd = 783317, XrefRangeStart = 783314, XrefRangeEnd = 783316, MetadataInitTokenRva = 47238784L, MetadataInitFlagRva = 59849635L)]
	public unsafe void FlipV()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FlipV_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783320, RefRangeEnd = 783321, XrefRangeStart = 783317, XrefRangeEnd = 783320, MetadataInitTokenRva = 47238740L, MetadataInitFlagRva = 59849636L)]
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

	public Atlas(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
