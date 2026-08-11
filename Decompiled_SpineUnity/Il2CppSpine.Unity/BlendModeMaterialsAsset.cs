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

public class BlendModeMaterialsAsset : SkeletonDataModifierAsset
{
	public class AtlasMaterialCache : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_cache;

		private static readonly System.IntPtr NativeMethodInfoPtr_CloneAtlasRegionWithMaterial_Public_AtlasRegion_AtlasRegion_Material_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetAtlasPageWithMaterial_Private_AtlasPage_AtlasPage_Material_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe Dictionary<KeyValuePair<AtlasPage, Material>, AtlasPage> cache
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cache);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<KeyValuePair<AtlasPage, Material>, AtlasPage>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cache)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		static AtlasMaterialCache()
		{
			Il2CppClassPointerStore<AtlasMaterialCache>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr, "AtlasMaterialCache");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AtlasMaterialCache>.NativeClassPtr);
			NativeFieldInfoPtr_cache = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasMaterialCache>.NativeClassPtr, "cache");
			NativeMethodInfoPtr_CloneAtlasRegionWithMaterial_Public_AtlasRegion_AtlasRegion_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasMaterialCache>.NativeClassPtr, 100664385);
			NativeMethodInfoPtr_GetAtlasPageWithMaterial_Private_AtlasPage_AtlasPage_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasMaterialCache>.NativeClassPtr, 100664386);
			NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasMaterialCache>.NativeClassPtr, 100664387);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasMaterialCache>.NativeClassPtr, 100664388);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 793297, RefRangeEnd = 793298, XrefRangeStart = 793294, XrefRangeEnd = 793297, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe AtlasRegion CloneAtlasRegionWithMaterial(AtlasRegion originalRegion, Material materialTemplate)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)originalRegion);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialTemplate);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CloneAtlasRegionWithMaterial_Public_AtlasRegion_AtlasRegion_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasRegion>(intPtr) : null;
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 793318, RefRangeEnd = 793319, XrefRangeStart = 793298, XrefRangeEnd = 793318, MetadataInitTokenRva = 47237272L, MetadataInitFlagRva = 59827128L)]
		public unsafe AtlasPage GetAtlasPageWithMaterial(AtlasPage originalPage, Material materialTemplate)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)originalPage);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialTemplate);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAtlasPageWithMaterial_Private_AtlasPage_AtlasPage_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AtlasPage>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793319, XrefRangeEnd = 793323, MetadataInitTokenRva = 47237236L, MetadataInitFlagRva = 59827129L)]
		public unsafe virtual void Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793323, XrefRangeEnd = 793330, MetadataInitTokenRva = 47237320L, MetadataInitFlagRva = 59827130L)]
		public unsafe AtlasMaterialCache()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AtlasMaterialCache>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public AtlasMaterialCache(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_multiplyMaterialTemplate;

	private static readonly System.IntPtr NativeFieldInfoPtr_screenMaterialTemplate;

	private static readonly System.IntPtr NativeFieldInfoPtr_additiveMaterialTemplate;

	private static readonly System.IntPtr NativeFieldInfoPtr_applyAdditiveMaterial;

	private static readonly System.IntPtr NativeMethodInfoPtr_Apply_Public_Virtual_Void_SkeletonData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyMaterials_Public_Static_Void_SkeletonData_Material_Material_Material_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Material multiplyMaterialTemplate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_multiplyMaterialTemplate);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_multiplyMaterialTemplate)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Material screenMaterialTemplate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_screenMaterialTemplate);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_screenMaterialTemplate)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Material additiveMaterialTemplate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_additiveMaterialTemplate);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_additiveMaterialTemplate)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe bool applyAdditiveMaterial
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_applyAdditiveMaterial);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_applyAdditiveMaterial)) = flag;
		}
	}

	static BlendModeMaterialsAsset()
	{
		Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "BlendModeMaterialsAsset");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr);
		NativeFieldInfoPtr_multiplyMaterialTemplate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr, "multiplyMaterialTemplate");
		NativeFieldInfoPtr_screenMaterialTemplate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr, "screenMaterialTemplate");
		NativeFieldInfoPtr_additiveMaterialTemplate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr, "additiveMaterialTemplate");
		NativeFieldInfoPtr_applyAdditiveMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr, "applyAdditiveMaterial");
		NativeMethodInfoPtr_Apply_Public_Virtual_Void_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr, 100664382);
		NativeMethodInfoPtr_ApplyMaterials_Public_Static_Void_SkeletonData_Material_Material_Material_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr, 100664383);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr, 100664384);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793330, XrefRangeEnd = 793331, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Apply(SkeletonData skeletonData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Apply_Public_Virtual_Void_SkeletonData_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 793384, RefRangeEnd = 793385, XrefRangeStart = 793331, XrefRangeEnd = 793384, MetadataInitTokenRva = 46323832L, MetadataInitFlagRva = 59827127L)]
	public unsafe static void ApplyMaterials(SkeletonData skeletonData, Material multiplyTemplate, Material screenTemplate, Material additiveTemplate, bool includeAdditiveSlots)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)multiplyTemplate);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)screenTemplate);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)additiveTemplate);
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &includeAdditiveSlots;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyMaterials_Public_Static_Void_SkeletonData_Material_Material_Material_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793385, XrefRangeEnd = 793386, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe BlendModeMaterialsAsset()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BlendModeMaterialsAsset>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BlendModeMaterialsAsset(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
