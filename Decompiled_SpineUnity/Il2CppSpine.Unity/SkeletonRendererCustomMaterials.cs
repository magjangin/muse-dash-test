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

public class SkeletonRendererCustomMaterials : MonoBehaviour
{
	[System.Serializable]
	public sealed class SlotMaterialOverride : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_overrideDisabled;

		private static readonly System.IntPtr NativeFieldInfoPtr_slotName;

		private static readonly System.IntPtr NativeFieldInfoPtr_material;

		private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_SlotMaterialOverride_0;

		public unsafe bool overrideDisabled
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideDisabled);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideDisabled)) = flag;
			}
		}

		public unsafe string slotName
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotName);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotName)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe Material material
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_material);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_material)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
			}
		}

		static SlotMaterialOverride()
		{
			Il2CppClassPointerStore<SlotMaterialOverride>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, "SlotMaterialOverride");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SlotMaterialOverride>.NativeClassPtr);
			NativeFieldInfoPtr_overrideDisabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotMaterialOverride>.NativeClassPtr, "overrideDisabled");
			NativeFieldInfoPtr_slotName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotMaterialOverride>.NativeClassPtr, "slotName");
			NativeFieldInfoPtr_material = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotMaterialOverride>.NativeClassPtr, "material");
			NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_SlotMaterialOverride_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotMaterialOverride>.NativeClassPtr, 100664252);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791772, XrefRangeEnd = 791777, MetadataInitTokenRva = 46284984L, MetadataInitFlagRva = 59848176L)]
		public unsafe virtual bool Equals(SlotMaterialOverride other)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)other));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_SlotMaterialOverride_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public SlotMaterialOverride(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public SlotMaterialOverride()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SlotMaterialOverride>.NativeClassPtr))
		{
		}
	}

	[System.Serializable]
	public sealed class AtlasMaterialOverride : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_overrideDisabled;

		private static readonly System.IntPtr NativeFieldInfoPtr_originalMaterial;

		private static readonly System.IntPtr NativeFieldInfoPtr_replacementMaterial;

		private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AtlasMaterialOverride_0;

		public unsafe bool overrideDisabled
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideDisabled);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_overrideDisabled)) = flag;
			}
		}

		public unsafe Material originalMaterial
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalMaterial);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_originalMaterial)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
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
			Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, "AtlasMaterialOverride");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr);
			NativeFieldInfoPtr_overrideDisabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr, "overrideDisabled");
			NativeFieldInfoPtr_originalMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr, "originalMaterial");
			NativeFieldInfoPtr_replacementMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr, "replacementMaterial");
			NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AtlasMaterialOverride_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AtlasMaterialOverride>.NativeClassPtr, 100664253);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791777, XrefRangeEnd = 791784, MetadataInitTokenRva = 47237364L, MetadataInitFlagRva = 59848175L)]
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

	private static readonly System.IntPtr NativeFieldInfoPtr_skeletonRenderer;

	private static readonly System.IntPtr NativeFieldInfoPtr_customSlotMaterials;

	private static readonly System.IntPtr NativeFieldInfoPtr_customMaterialOverrides;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetCustomMats_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetCustomSlotMaterials_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveCustomSlotMaterials_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetCustomMaterialOverrides_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveCustomMaterialOverrides_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe SkeletonRenderer skeletonRenderer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonRenderer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonRenderer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonRenderer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonRenderer));
		}
	}

	public unsafe List<SlotMaterialOverride> customSlotMaterials
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customSlotMaterials);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SlotMaterialOverride>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_customSlotMaterials)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
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

	static SkeletonRendererCustomMaterials()
	{
		Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonRendererCustomMaterials");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr);
		NativeFieldInfoPtr_skeletonRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, "skeletonRenderer");
		NativeFieldInfoPtr_customSlotMaterials = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, "customSlotMaterials");
		NativeFieldInfoPtr_customMaterialOverrides = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, "customMaterialOverrides");
		NativeMethodInfoPtr_SetCustomMats_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, 100664244);
		NativeMethodInfoPtr_SetCustomSlotMaterials_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, 100664245);
		NativeMethodInfoPtr_RemoveCustomSlotMaterials_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, 100664246);
		NativeMethodInfoPtr_SetCustomMaterialOverrides_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, 100664247);
		NativeMethodInfoPtr_RemoveCustomMaterialOverrides_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, 100664248);
		NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, 100664249);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, 100664250);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr, 100664251);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 791793, RefRangeEnd = 791797, XrefRangeStart = 791784, XrefRangeEnd = 791793, MetadataInitTokenRva = 46274744L, MetadataInitFlagRva = 59848167L)]
	public unsafe void SetCustomMats(bool enable)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&enable);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCustomMats_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 791811, RefRangeEnd = 791813, XrefRangeStart = 791797, XrefRangeEnd = 791811, MetadataInitTokenRva = 46274788L, MetadataInitFlagRva = 59848168L)]
	public unsafe void SetCustomSlotMaterials()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCustomSlotMaterials_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 791832, RefRangeEnd = 791834, XrefRangeStart = 791813, XrefRangeEnd = 791832, MetadataInitTokenRva = 46274696L, MetadataInitFlagRva = 59848169L)]
	public unsafe void RemoveCustomSlotMaterials()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveCustomSlotMaterials_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 791846, RefRangeEnd = 791848, XrefRangeStart = 791834, XrefRangeEnd = 791846, MetadataInitTokenRva = 46274712L, MetadataInitFlagRva = 59848170L)]
	public unsafe void SetCustomMaterialOverrides()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCustomMaterialOverrides_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 791865, RefRangeEnd = 791867, XrefRangeStart = 791848, XrefRangeEnd = 791865, MetadataInitTokenRva = 46274624L, MetadataInitFlagRva = 59848171L)]
	public unsafe void RemoveCustomMaterialOverrides()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveCustomMaterialOverrides_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791867, XrefRangeEnd = 791884, MetadataInitTokenRva = 46274584L, MetadataInitFlagRva = 59848172L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791884, XrefRangeEnd = 791894, MetadataInitTokenRva = 46274576L, MetadataInitFlagRva = 59848173L)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791894, XrefRangeEnd = 791906, MetadataInitTokenRva = 46274840L, MetadataInitFlagRva = 59848174L)]
	public unsafe SkeletonRendererCustomMaterials()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonRendererCustomMaterials>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonRendererCustomMaterials(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
