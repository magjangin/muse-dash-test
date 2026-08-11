using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SpineAtlasAsset : AtlasAssetBase
{
	private static readonly IntPtr NativeFieldInfoPtr_atlasFile;

	private static readonly IntPtr NativeFieldInfoPtr_materials;

	private static readonly IntPtr NativeFieldInfoPtr_atlas;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsLoaded_Public_Virtual_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Materials_Public_Virtual_get_IEnumerable_1_Material_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_MaterialCount_Public_Virtual_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_PrimaryMaterial_Public_Virtual_get_Material_0;

	private static readonly IntPtr NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineAtlasAsset_TextAsset_Il2CppReferenceArray_1_Material_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineAtlasAsset_TextAsset_Il2CppReferenceArray_1_Texture2D_Material_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineAtlasAsset_TextAsset_Il2CppReferenceArray_1_Texture2D_Shader_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Clear_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetAtlas_Public_Virtual_Atlas_0;

	private static readonly IntPtr NativeMethodInfoPtr_GenerateMesh_Public_Mesh_String_Mesh_byref_Material_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe TextAsset atlasFile
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlasFile);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextAsset>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlasFile)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textAsset));
		}
	}

	public unsafe Il2CppReferenceArray<Material> materials
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materials);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Material>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materials)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Atlas atlas
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlas);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Atlas>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlas)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlas));
		}
	}

	public unsafe override bool IsLoaded
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_IsLoaded_Public_Virtual_get_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe override IEnumerable<Material> Materials
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 37499, RefRangeEnd = 37502, XrefRangeStart = 37499, XrefRangeEnd = 37502, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Materials_Public_Virtual_get_IEnumerable_1_Material_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<Material>>(intPtr) : null;
		}
	}

	public unsafe override int MaterialCount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_MaterialCount_Public_Virtual_get_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe override Material PrimaryMaterial
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_PrimaryMaterial_Public_Virtual_get_Material_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
	}

	static SpineAtlasAsset()
	{
		Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SpineAtlasAsset");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr);
		NativeFieldInfoPtr_atlasFile = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, "atlasFile");
		NativeFieldInfoPtr_materials = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, "materials");
		NativeFieldInfoPtr_atlas = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, "atlas");
		NativeMethodInfoPtr_get_IsLoaded_Public_Virtual_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663889);
		NativeMethodInfoPtr_get_Materials_Public_Virtual_get_IEnumerable_1_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663890);
		NativeMethodInfoPtr_get_MaterialCount_Public_Virtual_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663891);
		NativeMethodInfoPtr_get_PrimaryMaterial_Public_Virtual_get_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663892);
		NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineAtlasAsset_TextAsset_Il2CppReferenceArray_1_Material_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663893);
		NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineAtlasAsset_TextAsset_Il2CppReferenceArray_1_Texture2D_Material_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663894);
		NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineAtlasAsset_TextAsset_Il2CppReferenceArray_1_Texture2D_Shader_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663895);
		NativeMethodInfoPtr_Reset_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663896);
		NativeMethodInfoPtr_Clear_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663897);
		NativeMethodInfoPtr_GetAtlas_Public_Virtual_Atlas_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663898);
		NativeMethodInfoPtr_GenerateMesh_Public_Mesh_String_Mesh_byref_Material_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663899);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr, 100663900);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788513, XrefRangeEnd = 788518, MetadataInitTokenRva = 46302972L, MetadataInitFlagRva = 59848229L)]
	public unsafe static SpineAtlasAsset CreateRuntimeInstance(TextAsset atlasText, Il2CppReferenceArray<Material> materials, bool initialize)
	{
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlasText);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materials);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &initialize;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineAtlasAsset_TextAsset_Il2CppReferenceArray_1_Material_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SpineAtlasAsset>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788558, RefRangeEnd = 788559, XrefRangeStart = 788518, XrefRangeEnd = 788558, MetadataInitTokenRva = 46303020L, MetadataInitFlagRva = 59848230L)]
	public unsafe static SpineAtlasAsset CreateRuntimeInstance(TextAsset atlasText, Il2CppReferenceArray<Texture2D> textures, Material materialPropertySource, bool initialize)
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlasText);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textures);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialPropertySource);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &initialize;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineAtlasAsset_TextAsset_Il2CppReferenceArray_1_Texture2D_Material_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SpineAtlasAsset>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788559, XrefRangeEnd = 788569, MetadataInitTokenRva = 46302988L, MetadataInitFlagRva = 59848231L)]
	public unsafe static SpineAtlasAsset CreateRuntimeInstance(TextAsset atlasText, Il2CppReferenceArray<Texture2D> textures, Shader shader, bool initialize)
	{
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlasText);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textures);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)shader);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &initialize;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineAtlasAsset_TextAsset_Il2CppReferenceArray_1_Texture2D_Shader_Boolean_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SpineAtlasAsset>(intPtr) : null;
	}

	[CallerCount(47)]
	[CachedScanResults(RefRangeStart = 101087, RefRangeEnd = 101134, XrefRangeStart = 101087, XrefRangeEnd = 101134, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Clear()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Clear_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788569, XrefRangeEnd = 788622, MetadataInitTokenRva = 46303128L, MetadataInitFlagRva = 59848232L)]
	public unsafe override Atlas GetAtlas()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetAtlas_Public_Virtual_Atlas_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Atlas>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788622, XrefRangeEnd = 788681, MetadataInitTokenRva = 46303068L, MetadataInitFlagRva = 59848233L)]
	public unsafe Mesh GenerateMesh(string name, Mesh mesh, out Material material, float scale = 0.01f)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mesh);
		byte* num = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)));
		nint num2 = 0;
		*(nint**)num = &num2;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &scale;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateMesh_Public_Mesh_String_Mesh_byref_Material_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		nint num3 = num2;
		material = ((num3 == 0) ? null : new Material(num3));
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Mesh>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 75347, RefRangeEnd = 75348, XrefRangeStart = 75347, XrefRangeEnd = 75348, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SpineAtlasAsset()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SpineAtlasAsset>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SpineAtlasAsset(IntPtr pointer)
		: base(pointer)
	{
	}
}
