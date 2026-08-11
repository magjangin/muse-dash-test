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

namespace Il2CppSpine.Unity.Deprecated;

public class SlotBlendModes : MonoBehaviour
{
	public sealed class MaterialTexturePair : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_texture2D;

		private static readonly System.IntPtr NativeFieldInfoPtr_material;

		public unsafe Texture2D texture2D
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_texture2D);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_texture2D)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
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

		static MaterialTexturePair()
		{
			Il2CppClassPointerStore<MaterialTexturePair>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, "MaterialTexturePair");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MaterialTexturePair>.NativeClassPtr);
			NativeFieldInfoPtr_texture2D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialTexturePair>.NativeClassPtr, "texture2D");
			NativeFieldInfoPtr_material = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialTexturePair>.NativeClassPtr, "material");
		}

		public MaterialTexturePair(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public MaterialTexturePair()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MaterialTexturePair>.NativeClassPtr))
		{
		}
	}

	public class MaterialWithRefcount : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_materialClone;

		private static readonly System.IntPtr NativeFieldInfoPtr_refcount;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Material_0;

		public unsafe Material materialClone
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materialClone);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_materialClone)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
			}
		}

		public unsafe int refcount
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_refcount);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_refcount)) = num;
			}
		}

		static MaterialWithRefcount()
		{
			Il2CppClassPointerStore<MaterialWithRefcount>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, "MaterialWithRefcount");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MaterialWithRefcount>.NativeClassPtr);
			NativeFieldInfoPtr_materialClone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialWithRefcount>.NativeClassPtr, "materialClone");
			NativeFieldInfoPtr_refcount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MaterialWithRefcount>.NativeClassPtr, "refcount");
			NativeMethodInfoPtr__ctor_Public_Void_Material_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MaterialWithRefcount>.NativeClassPtr, 100664457);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794279, XrefRangeEnd = 794281, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MaterialWithRefcount(Material mat)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MaterialWithRefcount>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mat);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Material_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public MaterialWithRefcount(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	public sealed class SlotMaterialTextureTuple : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_slot;

		private static readonly System.IntPtr NativeFieldInfoPtr_texture2D;

		private static readonly System.IntPtr NativeFieldInfoPtr_material;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Slot_Material_Texture2D_0;

		public unsafe Slot slot
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slot);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Slot>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slot)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot));
			}
		}

		public unsafe Texture2D texture2D
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_texture2D);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_texture2D)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
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

		static SlotMaterialTextureTuple()
		{
			Il2CppClassPointerStore<SlotMaterialTextureTuple>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, "SlotMaterialTextureTuple");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SlotMaterialTextureTuple>.NativeClassPtr);
			NativeFieldInfoPtr_slot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotMaterialTextureTuple>.NativeClassPtr, "slot");
			NativeFieldInfoPtr_texture2D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotMaterialTextureTuple>.NativeClassPtr, "texture2D");
			NativeFieldInfoPtr_material = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotMaterialTextureTuple>.NativeClassPtr, "material");
			NativeMethodInfoPtr__ctor_Public_Void_Slot_Material_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotMaterialTextureTuple>.NativeClassPtr, 100664458);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794281, XrefRangeEnd = 794284, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SlotMaterialTextureTuple(Slot slot, Material material, Texture2D texture)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SlotMaterialTextureTuple>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Slot_Material_Texture2D_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SlotMaterialTextureTuple(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public SlotMaterialTextureTuple()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SlotMaterialTextureTuple>.NativeClassPtr))
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_materialTable;

	private static readonly System.IntPtr NativeFieldInfoPtr_multiplyMaterialSource;

	private static readonly System.IntPtr NativeFieldInfoPtr_screenMaterialSource;

	private static readonly System.IntPtr NativeFieldInfoPtr_texture;

	private static readonly System.IntPtr NativeFieldInfoPtr_slotsWithCustomMaterial;

	private static readonly System.IntPtr NativeFieldInfoPtr__Applied_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_MaterialTable_Internal_Static_get_Dictionary_2_MaterialTexturePair_MaterialWithRefcount_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetOrAddMaterialFor_Internal_Static_Material_Material_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetExistingMaterialFor_Internal_Static_MaterialWithRefcount_Material_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RemoveMaterialFromTable_Internal_Static_Void_Material_Texture2D_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Applied_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Applied_Private_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Apply_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Remove_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetTexture_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static Dictionary<MaterialTexturePair, MaterialWithRefcount> materialTable
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_materialTable, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<MaterialTexturePair, MaterialWithRefcount>>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_materialTable, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Material multiplyMaterialSource
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_multiplyMaterialSource);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_multiplyMaterialSource)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Material screenMaterialSource
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_screenMaterialSource);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_screenMaterialSource)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe Texture2D texture
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_texture);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Texture2D>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_texture)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture2D));
		}
	}

	public unsafe Il2CppReferenceArray<SlotMaterialTextureTuple> slotsWithCustomMaterial
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotsWithCustomMaterial);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<SlotMaterialTextureTuple>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotsWithCustomMaterial)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool _Applied_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Applied_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Applied_k__BackingField)) = flag;
		}
	}

	public unsafe static Dictionary<MaterialTexturePair, MaterialWithRefcount> MaterialTable
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 794293, RefRangeEnd = 794297, XrefRangeStart = 794284, XrefRangeEnd = 794293, MetadataInitTokenRva = 46284888L, MetadataInitFlagRva = 59827156L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_MaterialTable_Internal_Static_get_Dictionary_2_MaterialTexturePair_MaterialWithRefcount_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<MaterialTexturePair, MaterialWithRefcount>>(intPtr) : null;
		}
	}

	public unsafe bool Applied
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Applied_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Applied_Private_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SlotBlendModes()
	{
		Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity.Deprecated", "SlotBlendModes");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr);
		NativeFieldInfoPtr_materialTable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, "materialTable");
		NativeFieldInfoPtr_multiplyMaterialSource = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, "multiplyMaterialSource");
		NativeFieldInfoPtr_screenMaterialSource = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, "screenMaterialSource");
		NativeFieldInfoPtr_texture = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, "texture");
		NativeFieldInfoPtr_slotsWithCustomMaterial = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, "slotsWithCustomMaterial");
		NativeFieldInfoPtr__Applied_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, "<Applied>k__BackingField");
		NativeMethodInfoPtr_get_MaterialTable_Internal_Static_get_Dictionary_2_MaterialTexturePair_MaterialWithRefcount_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664445);
		NativeMethodInfoPtr_GetOrAddMaterialFor_Internal_Static_Material_Material_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664446);
		NativeMethodInfoPtr_GetExistingMaterialFor_Internal_Static_MaterialWithRefcount_Material_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664447);
		NativeMethodInfoPtr_RemoveMaterialFromTable_Internal_Static_Void_Material_Texture2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664448);
		NativeMethodInfoPtr_get_Applied_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664449);
		NativeMethodInfoPtr_set_Applied_Private_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664450);
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664451);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664452);
		NativeMethodInfoPtr_Apply_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664453);
		NativeMethodInfoPtr_Remove_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664454);
		NativeMethodInfoPtr_GetTexture_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664455);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr, 100664456);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 794326, RefRangeEnd = 794328, XrefRangeStart = 794297, XrefRangeEnd = 794326, MetadataInitTokenRva = 46284700L, MetadataInitFlagRva = 59827157L)]
	public unsafe static Material GetOrAddMaterialFor(Material materialSource, Texture2D texture)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialSource);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetOrAddMaterialFor_Internal_Static_Material_Material_Texture2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794328, XrefRangeEnd = 794340, MetadataInitTokenRva = 46284664L, MetadataInitFlagRva = 59827158L)]
	public unsafe static MaterialWithRefcount GetExistingMaterialFor(Material materialSource, Texture2D texture)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialSource);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetExistingMaterialFor_Internal_Static_MaterialWithRefcount_Material_Texture2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MaterialWithRefcount>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 794346, RefRangeEnd = 794347, XrefRangeStart = 794340, XrefRangeEnd = 794346, MetadataInitTokenRva = 46284788L, MetadataInitFlagRva = 59827159L)]
	public unsafe static void RemoveMaterialFromTable(Material materialSource, Texture2D texture)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)materialSource);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)texture);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RemoveMaterialFromTable_Internal_Static_Void_Material_Texture2D_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794347, XrefRangeEnd = 794348, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794348, XrefRangeEnd = 794349, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 794405, RefRangeEnd = 794406, XrefRangeStart = 794349, XrefRangeEnd = 794405, MetadataInitTokenRva = 46284608L, MetadataInitFlagRva = 59827160L)]
	public unsafe void Apply()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Apply_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 794436, RefRangeEnd = 794437, XrefRangeStart = 794406, XrefRangeEnd = 794436, MetadataInitTokenRva = 46284824L, MetadataInitFlagRva = 59827161L)]
	public unsafe void Remove()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Remove_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 794458, RefRangeEnd = 794460, XrefRangeStart = 794437, XrefRangeEnd = 794458, MetadataInitTokenRva = 46284752L, MetadataInitFlagRva = 59827162L)]
	public unsafe void GetTexture()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTexture_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 794460, XrefRangeEnd = 794465, MetadataInitTokenRva = 46284856L, MetadataInitFlagRva = 59827163L)]
	public unsafe SlotBlendModes()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SlotBlendModes>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SlotBlendModes(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
