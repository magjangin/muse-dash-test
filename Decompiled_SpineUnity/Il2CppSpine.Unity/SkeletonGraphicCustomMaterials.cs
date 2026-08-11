using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SkeletonGraphicCustomMaterials : MonoBehaviour
{
	[System.Serializable]
	public sealed class AtlasMaterialOverride : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_overrideEnabled;

		private static readonly System.IntPtr NativeFieldInfoPtr_originalTexture;

		private static readonly System.IntPtr NativeFieldInfoPtr_replacementMaterial;

		private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AtlasMaterialOverride_0;

		public unsafe bool overrideEnabled
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideEnabled);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideEnabled)) = flag;
			}
		}

		public unsafe Texture originalTexture
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalTexture);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture));
			}
		}

		public unsafe Material replacementMaterial
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_replacementMaterial);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_replacementMaterial)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
			}
		}

		static AtlasMaterialOverride()
		{
			Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, "AtlasMaterialOverride");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr);
			NativeFieldInfoPtr_overrideEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr, "overrideEnabled");
			NativeFieldInfoPtr_originalTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr, "originalTexture");
			NativeFieldInfoPtr_replacementMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr, "replacementMaterial");
			NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AtlasMaterialOverride_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr, 100664242);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791641, XrefRangeEnd = 791648, MetadataInitTokenRva = 47237384L, MetadataInitFlagRva = 59848102L)]
		public unsafe virtual bool Equals(AtlasMaterialOverride other)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)other));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AtlasMaterialOverride_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public AtlasMaterialOverride(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public AtlasMaterialOverride()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr))
		{
		}
	}

	[System.Serializable]
	public sealed class AtlasTextureOverride : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_overrideEnabled;

		private static readonly System.IntPtr NativeFieldInfoPtr_originalTexture;

		private static readonly System.IntPtr NativeFieldInfoPtr_replacementTexture;

		private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AtlasTextureOverride_0;

		public unsafe bool overrideEnabled
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideEnabled);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideEnabled)) = flag;
			}
		}

		public unsafe Texture originalTexture
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalTexture);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture));
			}
		}

		public unsafe Texture replacementTexture
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_replacementTexture);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_replacementTexture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture));
			}
		}

		static AtlasTextureOverride()
		{
			Il2CppClassPointerStore<AtlasTextureOverride>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, "AtlasTextureOverride");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AtlasTextureOverride>.NativeClassPtr);
			NativeFieldInfoPtr_overrideEnabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasTextureOverride>.NativeClassPtr, "overrideEnabled");
			NativeFieldInfoPtr_originalTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasTextureOverride>.NativeClassPtr, "originalTexture");
			NativeFieldInfoPtr_replacementTexture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasTextureOverride>.NativeClassPtr, "replacementTexture");
			NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AtlasTextureOverride_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasTextureOverride>.NativeClassPtr, 100664243);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791648, XrefRangeEnd = 791655, MetadataInitTokenRva = 47237548L, MetadataInitFlagRva = 59848103L)]
		public unsafe virtual bool Equals(AtlasTextureOverride other)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)other));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AtlasTextureOverride_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public AtlasTextureOverride(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public AtlasTextureOverride()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AtlasTextureOverride>.NativeClassPtr))
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_skeletonGraphic;

	private static readonly System.IntPtr NativeFieldInfoPtr_customMaterialOverrides;

	private static readonly System.IntPtr NativeFieldInfoPtr_customTextureOverrides;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetCustomMaterialOverrides_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveCustomMaterialOverrides_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetCustomTextureOverrides_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveCustomTextureOverrides_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe SkeletonGraphic skeletonGraphic
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonGraphic);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonGraphic>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonGraphic)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonGraphic));
		}
	}

	public unsafe List<AtlasMaterialOverride> customMaterialOverrides
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customMaterialOverrides);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<AtlasMaterialOverride>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customMaterialOverrides)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<AtlasTextureOverride> customTextureOverrides
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customTextureOverrides);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<AtlasTextureOverride>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customTextureOverrides)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	static SkeletonGraphicCustomMaterials()
	{
		Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonGraphicCustomMaterials");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr);
		NativeFieldInfoPtr_skeletonGraphic = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, "skeletonGraphic");
		NativeFieldInfoPtr_customMaterialOverrides = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, "customMaterialOverrides");
		NativeFieldInfoPtr_customTextureOverrides = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, "customTextureOverrides");
		NativeMethodInfoPtr_SetCustomMaterialOverrides_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, 100664235);
		NativeMethodInfoPtr_RemoveCustomMaterialOverrides_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, 100664236);
		NativeMethodInfoPtr_SetCustomTextureOverrides_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, 100664237);
		NativeMethodInfoPtr_RemoveCustomTextureOverrides_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, 100664238);
		NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, 100664239);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, 100664240);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr, 100664241);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791655, XrefRangeEnd = 791667, MetadataInitTokenRva = 46271000L, MetadataInitFlagRva = 59848095L)]
	public unsafe void SetCustomMaterialOverrides()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCustomMaterialOverrides_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791684, RefRangeEnd = 791685, XrefRangeStart = 791667, XrefRangeEnd = 791684, MetadataInitTokenRva = 46270928L, MetadataInitFlagRva = 59848096L)]
	public unsafe void RemoveCustomMaterialOverrides()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveCustomMaterialOverrides_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791685, XrefRangeEnd = 791697, MetadataInitTokenRva = 46271032L, MetadataInitFlagRva = 59848097L)]
	public unsafe void SetCustomTextureOverrides()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCustomTextureOverrides_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 791714, RefRangeEnd = 791715, XrefRangeStart = 791697, XrefRangeEnd = 791714, MetadataInitTokenRva = 46270960L, MetadataInitFlagRva = 59848098L)]
	public unsafe void RemoveCustomTextureOverrides()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveCustomTextureOverrides_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791715, XrefRangeEnd = 791750, MetadataInitTokenRva = 46270892L, MetadataInitFlagRva = 59848099L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791750, XrefRangeEnd = 791760, MetadataInitTokenRva = 46270840L, MetadataInitFlagRva = 59848100L)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791760, XrefRangeEnd = 791772, MetadataInitTokenRva = 46271064L, MetadataInitFlagRva = 59848101L)]
	public unsafe SkeletonGraphicCustomMaterials()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonGraphicCustomMaterials>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonGraphicCustomMaterials(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
