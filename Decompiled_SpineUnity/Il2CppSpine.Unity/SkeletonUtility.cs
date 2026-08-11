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

public sealed class SkeletonUtility : MonoBehaviour
{
	public sealed class SkeletonUtilityDelegate : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static SkeletonUtilityDelegate()
		{
			Il2CppClassPointerStore<SkeletonUtilityDelegate>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "SkeletonUtilityDelegate");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityDelegate>.NativeClassPtr, 100664305);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityDelegate>.NativeClassPtr, 100664306);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityDelegate>.NativeClassPtr, 100664307);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtilityDelegate>.NativeClassPtr, 100664308);
		}

		[CallerCount(6192)]
		[CachedScanResults(RefRangeStart = 39733, RefRangeEnd = 45925, XrefRangeStart = 39733, XrefRangeEnd = 45925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe SkeletonUtilityDelegate(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonUtilityDelegate>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(422)]
		[CachedScanResults(RefRangeStart = 79940, RefRangeEnd = 80362, XrefRangeStart = 79940, XrefRangeEnd = 80362, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Invoke()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 80363, RefRangeEnd = 80365, XrefRangeStart = 80363, XrefRangeEnd = 80365, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.IAsyncResult>(intPtr) : null;
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 45955, RefRangeEnd = 45956, XrefRangeStart = 45955, XrefRangeEnd = 45956, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void EndInvoke(Il2CppSystem.IAsyncResult result)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)result);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public SkeletonUtilityDelegate(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator SkeletonUtilityDelegate(System.Action P_0)
		{
			return DelegateSupport.ConvertDelegate<SkeletonUtilityDelegate>((System.Delegate)P_0);
		}

		public static SkeletonUtilityDelegate operator +(SkeletonUtilityDelegate P_0, SkeletonUtilityDelegate P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<SkeletonUtilityDelegate>();
		}

		public static SkeletonUtilityDelegate operator -(SkeletonUtilityDelegate P_0, SkeletonUtilityDelegate P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<SkeletonUtilityDelegate>();
			}
			return (SkeletonUtilityDelegate)obj;
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_OnReset;

	private static readonly System.IntPtr NativeFieldInfoPtr_boneRoot;

	private static readonly System.IntPtr NativeFieldInfoPtr_flipBy180DegreeRotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_skeletonRenderer;

	private static readonly System.IntPtr NativeFieldInfoPtr_skeletonGraphic;

	private static readonly System.IntPtr NativeFieldInfoPtr_canvas;

	private static readonly System.IntPtr NativeFieldInfoPtr_skeletonAnimation;

	private static readonly System.IntPtr NativeFieldInfoPtr_skeletonComponent;

	private static readonly System.IntPtr NativeFieldInfoPtr_boneComponents;

	private static readonly System.IntPtr NativeFieldInfoPtr_constraintComponents;

	private static readonly System.IntPtr NativeFieldInfoPtr_positionScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_hasOverrideBones;

	private static readonly System.IntPtr NativeFieldInfoPtr_hasConstraints;

	private static readonly System.IntPtr NativeFieldInfoPtr_needToReprocessBones;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddBoundingBoxGameObject_Public_Static_PolygonCollider2D_Skeleton_String_String_String_Transform_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddBoundingBoxGameObject_Public_Static_PolygonCollider2D_String_BoundingBoxAttachment_Slot_Transform_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddBoundingBoxAsComponent_Public_Static_PolygonCollider2D_BoundingBoxAttachment_Slot_GameObject_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetColliderPointsLocal_Public_Static_Void_PolygonCollider2D_Slot_BoundingBoxAttachment_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBoundingBoxBounds_Public_Static_Bounds_BoundingBoxAttachment_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddBoneRigidbody2D_Public_Static_Rigidbody2D_GameObject_Boolean_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_OnReset_Public_add_Void_SkeletonUtilityDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnReset_Public_rem_Void_SkeletonUtilityDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SkeletonComponent_Public_get_ISkeletonComponent_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Skeleton_Public_get_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_PositionScale_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResubscribeEvents_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleRendererReset_Private_Void_SkeletonRenderer_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HandleRendererReset_Private_Void_SkeletonGraphic_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegisterBone_Public_Void_SkeletonUtilityBone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnregisterBone_Public_Void_SkeletonUtilityBone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RegisterConstraint_Public_Void_SkeletonUtilityConstraint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UnregisterConstraint_Public_Void_SkeletonUtilityConstraint_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CollectBones_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateLocal_Private_Void_ISkeletonAnimation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateWorld_Private_Void_ISkeletonAnimation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateComplete_Private_Void_ISkeletonAnimation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateAllBones_Private_Void_UpdatePhase_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBoneRoot_Public_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SpawnRoot_Public_GameObject_Mode_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SpawnHierarchy_Public_GameObject_Mode_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SpawnBoneRecursively_Public_GameObject_Bone_Transform_Mode_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SpawnBone_Public_GameObject_Bone_Transform_Mode_Boolean_Boolean_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe SkeletonUtilityDelegate OnReset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnReset);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonUtilityDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_OnReset)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonUtilityDelegate));
		}
	}

	public unsafe Transform boneRoot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_boneRoot);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_boneRoot)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)transform));
		}
	}

	public unsafe bool flipBy180DegreeRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_flipBy180DegreeRotation);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_flipBy180DegreeRotation)) = flag;
		}
	}

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

	public unsafe Canvas canvas
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_canvas);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Canvas>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_canvas)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvas));
		}
	}

	public unsafe ISkeletonAnimation skeletonAnimation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonAnimation);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ISkeletonAnimation>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonAnimation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonAnimation));
		}
	}

	public unsafe ISkeletonComponent skeletonComponent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonComponent);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ISkeletonComponent>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonComponent)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonComponent));
		}
	}

	public unsafe List<SkeletonUtilityBone> boneComponents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_boneComponents);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SkeletonUtilityBone>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_boneComponents)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<SkeletonUtilityConstraint> constraintComponents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_constraintComponents);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<SkeletonUtilityConstraint>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_constraintComponents)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe float positionScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_positionScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_positionScale)) = num;
		}
	}

	public unsafe bool hasOverrideBones
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasOverrideBones);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasOverrideBones)) = flag;
		}
	}

	public unsafe bool hasConstraints
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasConstraints);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasConstraints)) = flag;
		}
	}

	public unsafe bool needToReprocessBones
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_needToReprocessBones);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_needToReprocessBones)) = flag;
		}
	}

	public unsafe ISkeletonComponent SkeletonComponent
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792174, XrefRangeEnd = 792181, MetadataInitTokenRva = 46277764L, MetadataInitFlagRva = 59848206L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonComponent_Public_get_ISkeletonComponent_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ISkeletonComponent>(intPtr) : null;
		}
	}

	public unsafe Skeleton Skeleton
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792181, XrefRangeEnd = 792196, MetadataInitTokenRva = 46277772L, MetadataInitFlagRva = 59848207L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Skeleton_Public_get_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skeleton>(intPtr) : null;
		}
	}

	public unsafe bool IsValid
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792196, XrefRangeEnd = 792200, MetadataInitTokenRva = 46277704L, MetadataInitFlagRva = 59848208L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float PositionScale
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PositionScale_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SkeletonUtility()
	{
		Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonUtility");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr);
		NativeFieldInfoPtr_OnReset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "OnReset");
		NativeFieldInfoPtr_boneRoot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "boneRoot");
		NativeFieldInfoPtr_flipBy180DegreeRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "flipBy180DegreeRotation");
		NativeFieldInfoPtr_skeletonRenderer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "skeletonRenderer");
		NativeFieldInfoPtr_skeletonGraphic = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "skeletonGraphic");
		NativeFieldInfoPtr_canvas = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "canvas");
		NativeFieldInfoPtr_skeletonAnimation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "skeletonAnimation");
		NativeFieldInfoPtr_skeletonComponent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "skeletonComponent");
		NativeFieldInfoPtr_boneComponents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "boneComponents");
		NativeFieldInfoPtr_constraintComponents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "constraintComponents");
		NativeFieldInfoPtr_positionScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "positionScale");
		NativeFieldInfoPtr_hasOverrideBones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "hasOverrideBones");
		NativeFieldInfoPtr_hasConstraints = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "hasConstraints");
		NativeFieldInfoPtr_needToReprocessBones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, "needToReprocessBones");
		NativeMethodInfoPtr_AddBoundingBoxGameObject_Public_Static_PolygonCollider2D_Skeleton_String_String_String_Transform_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664271);
		NativeMethodInfoPtr_AddBoundingBoxGameObject_Public_Static_PolygonCollider2D_String_BoundingBoxAttachment_Slot_Transform_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664272);
		NativeMethodInfoPtr_AddBoundingBoxAsComponent_Public_Static_PolygonCollider2D_BoundingBoxAttachment_Slot_GameObject_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664273);
		NativeMethodInfoPtr_SetColliderPointsLocal_Public_Static_Void_PolygonCollider2D_Slot_BoundingBoxAttachment_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664274);
		NativeMethodInfoPtr_GetBoundingBoxBounds_Public_Static_Bounds_BoundingBoxAttachment_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664275);
		NativeMethodInfoPtr_AddBoneRigidbody2D_Public_Static_Rigidbody2D_GameObject_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664276);
		NativeMethodInfoPtr_add_OnReset_Public_add_Void_SkeletonUtilityDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664277);
		NativeMethodInfoPtr_remove_OnReset_Public_rem_Void_SkeletonUtilityDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664278);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664279);
		NativeMethodInfoPtr_get_SkeletonComponent_Public_get_ISkeletonComponent_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664280);
		NativeMethodInfoPtr_get_Skeleton_Public_get_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664281);
		NativeMethodInfoPtr_get_IsValid_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664282);
		NativeMethodInfoPtr_get_PositionScale_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664283);
		NativeMethodInfoPtr_ResubscribeEvents_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664284);
		NativeMethodInfoPtr_OnEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664285);
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664286);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664287);
		NativeMethodInfoPtr_HandleRendererReset_Private_Void_SkeletonRenderer_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664288);
		NativeMethodInfoPtr_HandleRendererReset_Private_Void_SkeletonGraphic_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664289);
		NativeMethodInfoPtr_RegisterBone_Public_Void_SkeletonUtilityBone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664290);
		NativeMethodInfoPtr_UnregisterBone_Public_Void_SkeletonUtilityBone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664291);
		NativeMethodInfoPtr_RegisterConstraint_Public_Void_SkeletonUtilityConstraint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664292);
		NativeMethodInfoPtr_UnregisterConstraint_Public_Void_SkeletonUtilityConstraint_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664293);
		NativeMethodInfoPtr_CollectBones_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664294);
		NativeMethodInfoPtr_UpdateLocal_Private_Void_ISkeletonAnimation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664295);
		NativeMethodInfoPtr_UpdateWorld_Private_Void_ISkeletonAnimation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664296);
		NativeMethodInfoPtr_UpdateComplete_Private_Void_ISkeletonAnimation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664297);
		NativeMethodInfoPtr_UpdateAllBones_Private_Void_UpdatePhase_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664298);
		NativeMethodInfoPtr_GetBoneRoot_Public_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664299);
		NativeMethodInfoPtr_SpawnRoot_Public_GameObject_Mode_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664300);
		NativeMethodInfoPtr_SpawnHierarchy_Public_GameObject_Mode_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664301);
		NativeMethodInfoPtr_SpawnBoneRecursively_Public_GameObject_Bone_Transform_Mode_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664302);
		NativeMethodInfoPtr_SpawnBone_Public_GameObject_Bone_Transform_Mode_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664303);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr, 100664304);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 792074, RefRangeEnd = 792075, XrefRangeStart = 792058, XrefRangeEnd = 792074, MetadataInitTokenRva = 46276960L, MetadataInitFlagRva = 59848198L)]
	public unsafe static PolygonCollider2D AddBoundingBoxGameObject(Skeleton skeleton, string skinName, string slotName, string attachmentName, Transform parent, bool isTrigger = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(skinName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(slotName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attachmentName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parent);
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &isTrigger;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddBoundingBoxGameObject_Public_Static_PolygonCollider2D_Skeleton_String_String_String_Transform_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PolygonCollider2D>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792075, XrefRangeEnd = 792107, MetadataInitTokenRva = 46276916L, MetadataInitFlagRva = 59848199L)]
	public unsafe static PolygonCollider2D AddBoundingBoxGameObject(string name, BoundingBoxAttachment box, Slot slot, Transform parent, bool isTrigger = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)box);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parent);
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &isTrigger;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddBoundingBoxGameObject_Public_Static_PolygonCollider2D_String_BoundingBoxAttachment_Slot_Transform_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PolygonCollider2D>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792107, XrefRangeEnd = 792120, MetadataInitTokenRva = 46276888L, MetadataInitFlagRva = 59848200L)]
	public unsafe static PolygonCollider2D AddBoundingBoxAsComponent(BoundingBoxAttachment box, Slot slot, GameObject gameObject, bool isTrigger = true)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)box);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &isTrigger;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddBoundingBoxAsComponent_Public_Static_PolygonCollider2D_BoundingBoxAttachment_Slot_GameObject_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PolygonCollider2D>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792131, RefRangeEnd = 792133, XrefRangeStart = 792120, XrefRangeEnd = 792131, MetadataInitTokenRva = 46277236L, MetadataInitFlagRva = 59848201L)]
	public unsafe static void SetColliderPointsLocal(PolygonCollider2D collider, Slot slot, BoundingBoxAttachment box, float scale = 1f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collider);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)box);
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &scale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetColliderPointsLocal_Public_Static_Void_PolygonCollider2D_Slot_BoundingBoxAttachment_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792133, XrefRangeEnd = 792139, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Bounds GetBoundingBoxBounds(BoundingBoxAttachment boundingBox, float depth = 0f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)boundingBox);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &depth;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBoundingBoxBounds_Public_Static_Bounds_BoundingBoxAttachment_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Bounds*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792139, XrefRangeEnd = 792149, MetadataInitTokenRva = 46276816L, MetadataInitFlagRva = 59848202L)]
	public unsafe static Rigidbody2D AddBoneRigidbody2D(GameObject gameObject, bool isKinematic = true, float gravityScale = 0f)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &isKinematic;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &gravityScale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddBoneRigidbody2D_Public_Static_Rigidbody2D_GameObject_Boolean_Single_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Rigidbody2D>(intPtr) : null;
	}

	[SpecialName]
	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792153, RefRangeEnd = 792155, XrefRangeStart = 792149, XrefRangeEnd = 792153, MetadataInitTokenRva = 46277672L, MetadataInitFlagRva = 59848203L)]
	public unsafe void add_OnReset(SkeletonUtilityDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnReset_Public_add_Void_SkeletonUtilityDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792159, RefRangeEnd = 792161, XrefRangeStart = 792155, XrefRangeEnd = 792159, MetadataInitTokenRva = 46277820L, MetadataInitFlagRva = 59848204L)]
	public unsafe void remove_OnReset(SkeletonUtilityDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnReset_Public_rem_Void_SkeletonUtilityDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792161, XrefRangeEnd = 792174, MetadataInitTokenRva = 46277588L, MetadataInitFlagRva = 59848205L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792200, XrefRangeEnd = 792202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResubscribeEvents()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResubscribeEvents_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 792293, RefRangeEnd = 792294, XrefRangeStart = 792202, XrefRangeEnd = 792293, MetadataInitTokenRva = 46277096L, MetadataInitFlagRva = 59848209L)]
	public unsafe void OnEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792294, XrefRangeEnd = 792295, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 792330, RefRangeEnd = 792331, XrefRangeStart = 792295, XrefRangeEnd = 792330, MetadataInitTokenRva = 46277068L, MetadataInitFlagRva = 59848210L)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792331, XrefRangeEnd = 792333, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HandleRendererReset(SkeletonRenderer r)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)r);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleRendererReset_Private_Void_SkeletonRenderer_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HandleRendererReset(SkeletonGraphic g)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)g);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleRendererReset_Private_Void_SkeletonGraphic_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792333, XrefRangeEnd = 792338, MetadataInitTokenRva = 46277164L, MetadataInitFlagRva = 59848211L)]
	public unsafe void RegisterBone(SkeletonUtilityBone bone)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterBone_Public_Void_SkeletonUtilityBone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792338, XrefRangeEnd = 792342, MetadataInitTokenRva = 46277376L, MetadataInitFlagRva = 59848212L)]
	public unsafe void UnregisterBone(SkeletonUtilityBone bone)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnregisterBone_Public_Void_SkeletonUtilityBone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792342, XrefRangeEnd = 792347, MetadataInitTokenRva = 46277180L, MetadataInitFlagRva = 59848213L)]
	public unsafe void RegisterConstraint(SkeletonUtilityConstraint constraint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)constraint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterConstraint_Public_Void_SkeletonUtilityConstraint_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792347, XrefRangeEnd = 792351, MetadataInitTokenRva = 46277440L, MetadataInitFlagRva = 59848214L)]
	public unsafe void UnregisterConstraint(SkeletonUtilityConstraint constraint)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)constraint);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnregisterConstraint_Public_Void_SkeletonUtilityConstraint_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 792361, RefRangeEnd = 792369, XrefRangeStart = 792351, XrefRangeEnd = 792361, MetadataInitTokenRva = 46277000L, MetadataInitFlagRva = 59848215L)]
	public unsafe void CollectBones()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CollectBones_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792369, XrefRangeEnd = 792373, MetadataInitTokenRva = 46277496L, MetadataInitFlagRva = 59848216L)]
	public unsafe void UpdateLocal(ISkeletonAnimation anim)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)anim);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateLocal_Private_Void_ISkeletonAnimation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792373, XrefRangeEnd = 792376, MetadataInitTokenRva = 46277544L, MetadataInitFlagRva = 59848217L)]
	public unsafe void UpdateWorld(ISkeletonAnimation anim)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)anim);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateWorld_Private_Void_ISkeletonAnimation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792376, XrefRangeEnd = 792377, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateComplete(ISkeletonAnimation anim)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)anim);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateComplete_Private_Void_ISkeletonAnimation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 792384, RefRangeEnd = 792387, XrefRangeStart = 792377, XrefRangeEnd = 792384, MetadataInitTokenRva = 46277452L, MetadataInitFlagRva = 59848218L)]
	public unsafe void UpdateAllBones(SkeletonUtilityBone.UpdatePhase phase)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&phase);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateAllBones_Private_Void_UpdatePhase_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792414, RefRangeEnd = 792416, XrefRangeStart = 792387, XrefRangeEnd = 792414, MetadataInitTokenRva = 46277012L, MetadataInitFlagRva = 59848219L)]
	public unsafe Transform GetBoneRoot()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBoneRoot_Public_Transform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Transform>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792416, XrefRangeEnd = 792423, MetadataInitTokenRva = 46277356L, MetadataInitFlagRva = 59848220L)]
	public unsafe GameObject SpawnRoot(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&mode);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &rot;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &sca;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SpawnRoot_Public_GameObject_Mode_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792423, XrefRangeEnd = 792430, MetadataInitTokenRva = 46277324L, MetadataInitFlagRva = 59848221L)]
	public unsafe GameObject SpawnHierarchy(SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&mode);
		*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &rot;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &sca;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SpawnHierarchy_Public_GameObject_Mode_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792433, RefRangeEnd = 792435, XrefRangeStart = 792430, XrefRangeEnd = 792433, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameObject SpawnBoneRecursively(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parent);
		*(SkeletonUtilityBone.Mode**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &mode;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &rot;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &sca;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SpawnBoneRecursively_Public_GameObject_Bone_Transform_Mode_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 792460, RefRangeEnd = 792462, XrefRangeStart = 792435, XrefRangeEnd = 792460, MetadataInitTokenRva = 46277268L, MetadataInitFlagRva = 59848222L)]
	public unsafe GameObject SpawnBone(Bone bone, Transform parent, SkeletonUtilityBone.Mode mode, bool pos, bool rot, bool sca)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[6];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parent);
		*(SkeletonUtilityBone.Mode**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &mode;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &rot;
		*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &sca;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SpawnBone_Public_GameObject_Bone_Transform_Mode_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 792462, XrefRangeEnd = 792474, MetadataInitTokenRva = 46277624L, MetadataInitFlagRva = 59848223L)]
	public unsafe SkeletonUtility()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonUtility>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonUtility(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
