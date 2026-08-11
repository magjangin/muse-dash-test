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

public class BoundingBoxFollowerGraphic : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_DebugMessages;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonGraphic;

	private static readonly IntPtr NativeFieldInfoPtr_slotName;

	private static readonly IntPtr NativeFieldInfoPtr_isTrigger;

	private static readonly IntPtr NativeFieldInfoPtr_clearStateOnDisable;

	private static readonly IntPtr NativeFieldInfoPtr_slot;

	private static readonly IntPtr NativeFieldInfoPtr_currentAttachment;

	private static readonly IntPtr NativeFieldInfoPtr_currentAttachmentName;

	private static readonly IntPtr NativeFieldInfoPtr_currentCollider;

	private static readonly IntPtr NativeFieldInfoPtr_colliderTable;

	private static readonly IntPtr NativeFieldInfoPtr_nameTable;

	private static readonly IntPtr NativeMethodInfoPtr_get_Slot_Public_get_Slot_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CurrentAttachment_Public_get_BoundingBoxAttachment_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CurrentAttachmentName_Public_get_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_CurrentCollider_Public_get_PolygonCollider2D_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_IsTrigger_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_HandleRebuild_Private_Void_SkeletonGraphic_0;

	private static readonly IntPtr NativeMethodInfoPtr_Initialize_Public_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddCollidersForSkin_Private_Void_Skin_Int32_Il2CppReferenceArray_1_PolygonCollider2D_Single_byref_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ClearState_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_DisposeExcessCollidersAfter_Private_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_MatchAttachment_Private_Void_Attachment_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static bool DebugMessages
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DebugMessages, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DebugMessages, (void*)(&flag));
		}
	}

	public unsafe SkeletonGraphic skeletonGraphic
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonGraphic);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonGraphic>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonGraphic)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonGraphic));
		}
	}

	public unsafe string slotName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool isTrigger
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isTrigger);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isTrigger)) = flag;
		}
	}

	public unsafe bool clearStateOnDisable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clearStateOnDisable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clearStateOnDisable)) = flag;
		}
	}

	public unsafe Slot slot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slot);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Slot>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slot)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot));
		}
	}

	public unsafe BoundingBoxAttachment currentAttachment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentAttachment);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<BoundingBoxAttachment>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentAttachment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)boundingBoxAttachment));
		}
	}

	public unsafe string currentAttachmentName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentAttachmentName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentAttachmentName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe PolygonCollider2D currentCollider
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentCollider);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PolygonCollider2D>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_currentCollider)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)polygonCollider2D));
		}
	}

	public unsafe Dictionary<BoundingBoxAttachment, PolygonCollider2D> colliderTable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_colliderTable);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<BoundingBoxAttachment, PolygonCollider2D>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_colliderTable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<BoundingBoxAttachment, string> nameTable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nameTable);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<BoundingBoxAttachment, string>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nameTable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Slot Slot
	{
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 37367, RefRangeEnd = 37377, XrefRangeStart = 37367, XrefRangeEnd = 37377, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Slot_Public_get_Slot_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Slot>(intPtr) : null;
		}
	}

	public unsafe BoundingBoxAttachment CurrentAttachment
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 51248, RefRangeEnd = 51256, XrefRangeStart = 51248, XrefRangeEnd = 51256, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentAttachment_Public_get_BoundingBoxAttachment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<BoundingBoxAttachment>(intPtr) : null;
		}
	}

	public unsafe string CurrentAttachmentName
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 37591, RefRangeEnd = 37595, XrefRangeStart = 37591, XrefRangeEnd = 37595, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentAttachmentName_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	public unsafe PolygonCollider2D CurrentCollider
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_CurrentCollider_Public_get_PolygonCollider2D_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PolygonCollider2D>(intPtr) : null;
		}
	}

	public unsafe bool IsTrigger
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsTrigger_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static BoundingBoxFollowerGraphic()
	{
		Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "BoundingBoxFollowerGraphic");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr);
		NativeFieldInfoPtr_DebugMessages = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "DebugMessages");
		NativeFieldInfoPtr_skeletonGraphic = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "skeletonGraphic");
		NativeFieldInfoPtr_slotName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "slotName");
		NativeFieldInfoPtr_isTrigger = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "isTrigger");
		NativeFieldInfoPtr_clearStateOnDisable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "clearStateOnDisable");
		NativeFieldInfoPtr_slot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "slot");
		NativeFieldInfoPtr_currentAttachment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "currentAttachment");
		NativeFieldInfoPtr_currentAttachmentName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "currentAttachmentName");
		NativeFieldInfoPtr_currentCollider = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "currentCollider");
		NativeFieldInfoPtr_colliderTable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "colliderTable");
		NativeFieldInfoPtr_nameTable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, "nameTable");
		NativeMethodInfoPtr_get_Slot_Public_get_Slot_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663950);
		NativeMethodInfoPtr_get_CurrentAttachment_Public_get_BoundingBoxAttachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663951);
		NativeMethodInfoPtr_get_CurrentAttachmentName_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663952);
		NativeMethodInfoPtr_get_CurrentCollider_Public_get_PolygonCollider2D_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663953);
		NativeMethodInfoPtr_get_IsTrigger_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663954);
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663955);
		NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663956);
		NativeMethodInfoPtr_HandleRebuild_Private_Void_SkeletonGraphic_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663957);
		NativeMethodInfoPtr_Initialize_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663958);
		NativeMethodInfoPtr_AddCollidersForSkin_Private_Void_Skin_Int32_Il2CppReferenceArray_1_PolygonCollider2D_Single_byref_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663959);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663960);
		NativeMethodInfoPtr_ClearState_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663961);
		NativeMethodInfoPtr_DisposeExcessCollidersAfter_Private_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663962);
		NativeMethodInfoPtr_LateUpdate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663963);
		NativeMethodInfoPtr_MatchAttachment_Private_Void_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663964);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr, 100663965);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789182, XrefRangeEnd = 789183, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789183, XrefRangeEnd = 789205, MetadataInitTokenRva = 46333736L, MetadataInitFlagRva = 59827147L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HandleRebuild(SkeletonGraphic sr)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sr);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleRebuild_Private_Void_SkeletonGraphic_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 789287, RefRangeEnd = 789290, XrefRangeStart = 789205, XrefRangeEnd = 789287, MetadataInitTokenRva = 46333608L, MetadataInitFlagRva = 59827148L)]
	public unsafe void Initialize(bool overwrite = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&overwrite);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 789332, RefRangeEnd = 789334, XrefRangeStart = 789290, XrefRangeEnd = 789332, MetadataInitTokenRva = 46333508L, MetadataInitFlagRva = 59827149L)]
	public unsafe void AddCollidersForSkin(Skin skin, int slotIndex, Il2CppReferenceArray<PolygonCollider2D> previousColliders, float scale, ref int collidersCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skin);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &slotIndex;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)previousColliders);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &scale;
		*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = Unsafe.AsPointer(ref collidersCount);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddCollidersForSkin_Private_Void_Skin_Int32_Il2CppReferenceArray_1_PolygonCollider2D_Single_byref_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789334, XrefRangeEnd = 789344, MetadataInitTokenRva = 46333716L, MetadataInitFlagRva = 59827150L)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 789359, RefRangeEnd = 789360, XrefRangeStart = 789344, XrefRangeEnd = 789359, MetadataInitTokenRva = 46333532L, MetadataInitFlagRva = 59827151L)]
	public unsafe void ClearState()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearState_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789360, XrefRangeEnd = 789370, MetadataInitTokenRva = 46333568L, MetadataInitFlagRva = 59827152L)]
	public unsafe void DisposeExcessCollidersAfter(int requiredCount)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&requiredCount);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DisposeExcessCollidersAfter_Private_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789370, XrefRangeEnd = 789371, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LateUpdate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 789415, RefRangeEnd = 789416, XrefRangeStart = 789371, XrefRangeEnd = 789415, MetadataInitTokenRva = 46333680L, MetadataInitFlagRva = 59827153L)]
	public unsafe void MatchAttachment(Attachment attachment)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MatchAttachment_Private_Void_Attachment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789416, XrefRangeEnd = 789428, MetadataInitTokenRva = 46333804L, MetadataInitFlagRva = 59827154L)]
	public unsafe BoundingBoxFollowerGraphic()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BoundingBoxFollowerGraphic>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public BoundingBoxFollowerGraphic(IntPtr pointer)
		: base(pointer)
	{
	}
}
