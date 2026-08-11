using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine.Unity;

public class MaterialsTextureLoader : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_atlasAsset;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_SpineAtlasAsset_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Load_Public_Virtual_Final_New_Void_AtlasPage_String_0;

	public unsafe SpineAtlasAsset atlasAsset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlasAsset);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SpineAtlasAsset>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlasAsset)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spineAtlasAsset));
		}
	}

	static MaterialsTextureLoader()
	{
		Il2CppClassPointerStore<MaterialsTextureLoader>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "MaterialsTextureLoader");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MaterialsTextureLoader>.NativeClassPtr);
		NativeFieldInfoPtr_atlasAsset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialsTextureLoader>.NativeClassPtr, "atlasAsset");
		NativeMethodInfoPtr__ctor_Public_Void_SpineAtlasAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialsTextureLoader>.NativeClassPtr, 100663901);
		NativeMethodInfoPtr_Load_Public_Virtual_Final_New_Void_AtlasPage_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialsTextureLoader>.NativeClassPtr, 100663902);
	}

	[CallerCount(65)]
	[CachedScanResults(RefRangeStart = 53781, RefRangeEnd = 53846, XrefRangeStart = 53781, XrefRangeEnd = 53846, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MaterialsTextureLoader(SpineAtlasAsset atlasAsset)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MaterialsTextureLoader>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlasAsset);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_SpineAtlasAsset_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788681, XrefRangeEnd = 788699, MetadataInitTokenRva = 46337852L, MetadataInitFlagRva = 59827171L)]
	public unsafe virtual void Load(AtlasPage page, string path)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)page);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(path);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Load_Public_Virtual_Final_New_Void_AtlasPage_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MaterialsTextureLoader(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
