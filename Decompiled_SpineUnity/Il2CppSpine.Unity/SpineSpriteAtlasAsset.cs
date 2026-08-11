using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Il2CppSpine.Unity;

public class SpineSpriteAtlasAsset : AtlasAssetBase
{
	[System.Serializable]
	public class SavedRegionInfo : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_x;

		private static readonly System.IntPtr NativeFieldInfoPtr_y;

		private static readonly System.IntPtr NativeFieldInfoPtr_width;

		private static readonly System.IntPtr NativeFieldInfoPtr_height;

		private static readonly System.IntPtr NativeFieldInfoPtr_packingRotation;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe float x
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_x);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_x)) = num;
			}
		}

		public unsafe float y
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_y);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_y)) = num;
			}
		}

		public unsafe float width
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_width);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_width)) = num;
			}
		}

		public unsafe float height
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_height);
				return *(float*)num;
			}
			set
			{
				*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_height)) = num;
			}
		}

		public unsafe SpritePackingRotation packingRotation
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_packingRotation);
				return *(SpritePackingRotation*)num;
			}
			set
			{
				*(SpritePackingRotation*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_packingRotation)) = spritePackingRotation;
			}
		}

		static SavedRegionInfo()
		{
			Il2CppClassPointerStore<SavedRegionInfo>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, "SavedRegionInfo");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SavedRegionInfo>.NativeClassPtr);
			NativeFieldInfoPtr_x = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SavedRegionInfo>.NativeClassPtr, "x");
			NativeFieldInfoPtr_y = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SavedRegionInfo>.NativeClassPtr, "y");
			NativeFieldInfoPtr_width = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SavedRegionInfo>.NativeClassPtr, "width");
			NativeFieldInfoPtr_height = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SavedRegionInfo>.NativeClassPtr, "height");
			NativeFieldInfoPtr_packingRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SavedRegionInfo>.NativeClassPtr, "packingRotation");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SavedRegionInfo>.NativeClassPtr, 100663916);
		}

		[CallerCount(2392)]
		[CachedScanResults(RefRangeStart = 18875, RefRangeEnd = 21267, XrefRangeStart = 18875, XrefRangeEnd = 21267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SavedRegionInfo()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SavedRegionInfo>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SavedRegionInfo(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_spriteAtlasFile;

	private static readonly System.IntPtr NativeFieldInfoPtr_materials;

	private static readonly System.IntPtr NativeFieldInfoPtr_atlas;

	private static readonly System.IntPtr NativeFieldInfoPtr_updateRegionsInPlayMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_savedRegions;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsLoaded_Public_Virtual_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Materials_Public_Virtual_get_IEnumerable_1_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MaterialCount_Public_Virtual_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_PrimaryMaterial_Public_Virtual_get_Material_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineSpriteAtlasAsset_SpriteAtlas_Il2CppReferenceArray_1_Material_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Clear_Public_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAtlas_Public_Virtual_Atlas_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AssignRegionsFromSavedRegions_Protected_Void_Il2CppReferenceArray_1_Sprite_Atlas_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadAtlas_Private_Atlas_SpriteAtlas_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AccessPackedTexture_Public_Static_Texture2D_Il2CppReferenceArray_1_Sprite_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AccessPackedSprites_Public_Static_Il2CppReferenceArray_1_Sprite_SpriteAtlas_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe SpriteAtlas spriteAtlasFile
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spriteAtlasFile);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SpriteAtlas>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_spriteAtlasFile)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spriteAtlas));
		}
	}

	public unsafe Il2CppReferenceArray<Material> materials
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materials);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Material>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materials)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Atlas atlas
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlas);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Atlas>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_atlas)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)atlas));
		}
	}

	public unsafe bool updateRegionsInPlayMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateRegionsInPlayMode);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_updateRegionsInPlayMode)) = flag;
		}
	}

	public unsafe Il2CppReferenceArray<SavedRegionInfo> savedRegions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_savedRegions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<SavedRegionInfo>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_savedRegions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe override bool IsLoaded
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_IsLoaded_Public_Virtual_get_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_Materials_Public_Virtual_get_IEnumerable_1_Material_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerable<Material>>(intPtr) : null;
		}
	}

	public unsafe override int MaterialCount
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_MaterialCount_Public_Virtual_get_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_PrimaryMaterial_Public_Virtual_get_Material_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
	}

	static SpineSpriteAtlasAsset()
	{
		Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SpineSpriteAtlasAsset");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr);
		NativeFieldInfoPtr_spriteAtlasFile = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, "spriteAtlasFile");
		NativeFieldInfoPtr_materials = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, "materials");
		NativeFieldInfoPtr_atlas = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, "atlas");
		NativeFieldInfoPtr_updateRegionsInPlayMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, "updateRegionsInPlayMode");
		NativeFieldInfoPtr_savedRegions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, "savedRegions");
		NativeMethodInfoPtr_get_IsLoaded_Public_Virtual_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663903);
		NativeMethodInfoPtr_get_Materials_Public_Virtual_get_IEnumerable_1_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663904);
		NativeMethodInfoPtr_get_MaterialCount_Public_Virtual_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663905);
		NativeMethodInfoPtr_get_PrimaryMaterial_Public_Virtual_get_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663906);
		NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineSpriteAtlasAsset_SpriteAtlas_Il2CppReferenceArray_1_Material_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663907);
		NativeMethodInfoPtr_Reset_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663908);
		NativeMethodInfoPtr_Clear_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663909);
		NativeMethodInfoPtr_GetAtlas_Public_Virtual_Atlas_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663910);
		NativeMethodInfoPtr_AssignRegionsFromSavedRegions_Protected_Void_Il2CppReferenceArray_1_Sprite_Atlas_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663911);
		NativeMethodInfoPtr_LoadAtlas_Private_Atlas_SpriteAtlas_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663912);
		NativeMethodInfoPtr_AccessPackedTexture_Public_Static_Texture2D_Il2CppReferenceArray_1_Sprite_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663913);
		NativeMethodInfoPtr_AccessPackedSprites_Public_Static_Il2CppReferenceArray_1_Sprite_SpriteAtlas_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663914);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr, 100663915);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788699, XrefRangeEnd = 788704, MetadataInitTokenRva = 46303852L, MetadataInitFlagRva = 59848237L)]
	public unsafe static SpineSpriteAtlasAsset CreateRuntimeInstance(SpriteAtlas spriteAtlasFile, Il2CppReferenceArray<Material> materials, bool initialize)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spriteAtlasFile);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materials);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &initialize;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateRuntimeInstance_Public_Static_SpineSpriteAtlasAsset_SpriteAtlas_Il2CppReferenceArray_1_Material_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SpineSpriteAtlasAsset>(intPtr) : null;
	}

	[CallerCount(47)]
	[CachedScanResults(RefRangeStart = 101087, RefRangeEnd = 101134, XrefRangeStart = 101087, XrefRangeEnd = 101134, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Clear()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Clear_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788704, XrefRangeEnd = 788736, MetadataInitTokenRva = 46303884L, MetadataInitFlagRva = 59848238L)]
	public unsafe override Atlas GetAtlas()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetAtlas_Public_Virtual_Atlas_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Atlas>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788752, RefRangeEnd = 788753, XrefRangeStart = 788736, XrefRangeEnd = 788752, MetadataInitTokenRva = 46303812L, MetadataInitFlagRva = 59848239L)]
	public unsafe void AssignRegionsFromSavedRegions(Il2CppReferenceArray<Sprite> sprites, Atlas usedAtlas)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprites);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)usedAtlas);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AssignRegionsFromSavedRegions_Protected_Void_Il2CppReferenceArray_1_Sprite_Atlas_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 788798, RefRangeEnd = 788799, XrefRangeStart = 788753, XrefRangeEnd = 788798, MetadataInitTokenRva = 46303900L, MetadataInitFlagRva = 59848240L)]
	public unsafe Atlas LoadAtlas(SpriteAtlas spriteAtlas)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spriteAtlas);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadAtlas_Private_Atlas_SpriteAtlas_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Atlas>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788799, XrefRangeEnd = 788803, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Texture2D AccessPackedTexture(Il2CppReferenceArray<Sprite> sprites)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprites);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AccessPackedTexture_Public_Static_Texture2D_Il2CppReferenceArray_1_Sprite_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788803, XrefRangeEnd = 788808, MetadataInitTokenRva = 46303768L, MetadataInitFlagRva = 59848241L)]
	public unsafe static Il2CppReferenceArray<Sprite> AccessPackedSprites(SpriteAtlas spriteAtlas)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)spriteAtlas);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AccessPackedSprites_Public_Static_Il2CppReferenceArray_1_Sprite_SpriteAtlas_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Sprite>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 75347, RefRangeEnd = 75348, XrefRangeStart = 75347, XrefRangeEnd = 75348, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe SpineSpriteAtlasAsset()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SpineSpriteAtlasAsset>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SpineSpriteAtlasAsset(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
