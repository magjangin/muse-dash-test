using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SkeletonRootMotionBase : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr_rootMotionBoneName;

	private static readonly IntPtr NativeFieldInfoPtr_transformPositionX;

	private static readonly IntPtr NativeFieldInfoPtr_transformPositionY;

	private static readonly IntPtr NativeFieldInfoPtr_rootMotionScaleX;

	private static readonly IntPtr NativeFieldInfoPtr_rootMotionScaleY;

	private static readonly IntPtr NativeFieldInfoPtr_rigidBody2D;

	private static readonly IntPtr NativeFieldInfoPtr_rigidBody;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonComponent;

	private static readonly IntPtr NativeFieldInfoPtr_rootMotionBone;

	private static readonly IntPtr NativeFieldInfoPtr_rootMotionBoneIndex;

	private static readonly IntPtr NativeFieldInfoPtr_topLevelBones;

	private static readonly IntPtr NativeFieldInfoPtr_rigidbodyDisplacement;

	private static readonly IntPtr NativeMethodInfoPtr_get_UsesRigidbody_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Reset_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_FixedUpdate_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Protected_Virtual_New_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_FindRigidbodyComponent_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_AdditionalScale_Protected_Virtual_New_get_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_CalculateAnimationsMovementDelta_Protected_Abstract_Virtual_New_Vector2_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetRemainingRootMotion_Public_Abstract_Virtual_New_Vector2_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetRootMotionBone_Public_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_AdjustRootMotionToDistance_Public_Void_Vector2_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetAnimationRootMotion_Public_Vector2_Animation_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetAnimationRootMotion_Public_Vector2_Single_Single_Animation_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetTimelineMovementDelta_Private_Vector2_Single_Single_TranslateTimeline_Animation_0;

	private static readonly IntPtr NativeMethodInfoPtr_GatherTopLevelBones_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_HandleUpdateLocal_Private_Void_ISkeletonAnimation_0;

	private static readonly IntPtr NativeMethodInfoPtr_AdjustMovementDeltaToConfiguration_Private_Void_byref_Vector2_Skeleton_0;

	private static readonly IntPtr NativeMethodInfoPtr_ApplyRootMotion_Private_Void_Vector2_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe string rootMotionBoneName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rootMotionBoneName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rootMotionBoneName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool transformPositionX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformPositionX);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformPositionX)) = flag;
		}
	}

	public unsafe bool transformPositionY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformPositionY);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformPositionY)) = flag;
		}
	}

	public unsafe float rootMotionScaleX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rootMotionScaleX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rootMotionScaleX)) = num;
		}
	}

	public unsafe float rootMotionScaleY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rootMotionScaleY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rootMotionScaleY)) = num;
		}
	}

	public unsafe Rigidbody2D rigidBody2D
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rigidBody2D);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Rigidbody2D>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rigidBody2D)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rigidbody2D));
		}
	}

	public unsafe Rigidbody rigidBody
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rigidBody);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Rigidbody>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rigidBody)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rigidbody));
		}
	}

	public unsafe ISkeletonComponent skeletonComponent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonComponent);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ISkeletonComponent>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonComponent)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonComponent));
		}
	}

	public unsafe Bone rootMotionBone
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rootMotionBone);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Bone>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rootMotionBone)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone));
		}
	}

	public unsafe int rootMotionBoneIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rootMotionBoneIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rootMotionBoneIndex)) = num;
		}
	}

	public unsafe List<Bone> topLevelBones
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_topLevelBones);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<Bone>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_topLevelBones)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Vector2 rigidbodyDisplacement
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rigidbodyDisplacement);
			return *(Vector2*)num;
		}
		set
		{
			*(Vector2*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rigidbodyDisplacement)) = vector;
		}
	}

	public unsafe bool UsesRigidbody
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789609, XrefRangeEnd = 789613, MetadataInitTokenRva = 46276344L, MetadataInitFlagRva = 59848186L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_UsesRigidbody_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe virtual float AdditionalScale
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_AdditionalScale_Protected_Virtual_New_get_Single_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static SkeletonRootMotionBase()
	{
		Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonRootMotionBase");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr);
		NativeFieldInfoPtr_rootMotionBoneName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "rootMotionBoneName");
		NativeFieldInfoPtr_transformPositionX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "transformPositionX");
		NativeFieldInfoPtr_transformPositionY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "transformPositionY");
		NativeFieldInfoPtr_rootMotionScaleX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "rootMotionScaleX");
		NativeFieldInfoPtr_rootMotionScaleY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "rootMotionScaleY");
		NativeFieldInfoPtr_rigidBody2D = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "rigidBody2D");
		NativeFieldInfoPtr_rigidBody = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "rigidBody");
		NativeFieldInfoPtr_skeletonComponent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "skeletonComponent");
		NativeFieldInfoPtr_rootMotionBone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "rootMotionBone");
		NativeFieldInfoPtr_rootMotionBoneIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "rootMotionBoneIndex");
		NativeFieldInfoPtr_topLevelBones = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "topLevelBones");
		NativeFieldInfoPtr_rigidbodyDisplacement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, "rigidbodyDisplacement");
		NativeMethodInfoPtr_get_UsesRigidbody_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100663990);
		NativeMethodInfoPtr_Reset_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100663991);
		NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100663992);
		NativeMethodInfoPtr_FixedUpdate_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100663993);
		NativeMethodInfoPtr_OnDisable_Protected_Virtual_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100663994);
		NativeMethodInfoPtr_FindRigidbodyComponent_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100663995);
		NativeMethodInfoPtr_get_AdditionalScale_Protected_Virtual_New_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100663996);
		NativeMethodInfoPtr_CalculateAnimationsMovementDelta_Protected_Abstract_Virtual_New_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100663997);
		NativeMethodInfoPtr_GetRemainingRootMotion_Public_Abstract_Virtual_New_Vector2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100663998);
		NativeMethodInfoPtr_SetRootMotionBone_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100663999);
		NativeMethodInfoPtr_AdjustRootMotionToDistance_Public_Void_Vector2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100664000);
		NativeMethodInfoPtr_GetAnimationRootMotion_Public_Vector2_Animation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100664001);
		NativeMethodInfoPtr_GetAnimationRootMotion_Public_Vector2_Single_Single_Animation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100664002);
		NativeMethodInfoPtr_GetTimelineMovementDelta_Private_Vector2_Single_Single_TranslateTimeline_Animation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100664003);
		NativeMethodInfoPtr_GatherTopLevelBones_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100664004);
		NativeMethodInfoPtr_HandleUpdateLocal_Private_Void_ISkeletonAnimation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100664005);
		NativeMethodInfoPtr_AdjustMovementDeltaToConfiguration_Private_Void_byref_Vector2_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100664006);
		NativeMethodInfoPtr_ApplyRootMotion_Private_Void_Vector2_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100664007);
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr, 100664008);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789613, XrefRangeEnd = 789614, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Reset_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 789640, RefRangeEnd = 789642, XrefRangeStart = 789614, XrefRangeEnd = 789640, MetadataInitTokenRva = 46276280L, MetadataInitFlagRva = 59848187L)]
	public unsafe virtual void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Start_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789642, XrefRangeEnd = 789669, MetadataInitTokenRva = 46275984L, MetadataInitFlagRva = 59848188L)]
	public unsafe virtual void FixedUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_FixedUpdate_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789669, XrefRangeEnd = 789673, MetadataInitTokenRva = 46276196L, MetadataInitFlagRva = 59848189L)]
	public unsafe virtual void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDisable_Protected_Virtual_New_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 789699, RefRangeEnd = 789702, XrefRangeStart = 789673, XrefRangeEnd = 789699, MetadataInitTokenRva = 46275976L, MetadataInitFlagRva = 59848190L)]
	public unsafe void FindRigidbodyComponent()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindRigidbodyComponent_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe virtual Vector2 CalculateAnimationsMovementDelta()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_CalculateAnimationsMovementDelta_Protected_Abstract_Virtual_New_Vector2_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe virtual Vector2 GetRemainingRootMotion(int trackIndex = 0)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&trackIndex);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_GetRemainingRootMotion_Public_Abstract_Virtual_New_Vector2_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789702, XrefRangeEnd = 789718, MetadataInitTokenRva = 46276240L, MetadataInitFlagRva = 59848191L)]
	public unsafe void SetRootMotionBone(string name)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRootMotionBone_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void AdjustRootMotionToDistance(Vector2 distanceToTarget, int trackIndex = 0)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)(&distanceToTarget);
		*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &trackIndex;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdjustRootMotionToDistance_Public_Void_Vector2_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789718, XrefRangeEnd = 789719, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Vector2 GetAnimationRootMotion(Animation animation)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAnimationRootMotion_Public_Vector2_Animation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 789745, RefRangeEnd = 789751, XrefRangeStart = 789719, XrefRangeEnd = 789745, MetadataInitTokenRva = 46276088L, MetadataInitFlagRva = 59848192L)]
	public unsafe Vector2 GetAnimationRootMotion(float startTime, float endTime, Animation animation)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&startTime);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &endTime;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAnimationRootMotion_Public_Vector2_Single_Single_Animation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789751, XrefRangeEnd = 789771, MetadataInitTokenRva = 46276132L, MetadataInitFlagRva = 59848193L)]
	public unsafe Vector2 GetTimelineMovementDelta(float startTime, float endTime, TranslateTimeline timeline, Animation animation)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = (nint)(&startTime);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &endTime;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)timeline);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetTimelineMovementDelta_Private_Vector2_Single_Single_TranslateTimeline_Animation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Vector2*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 789786, RefRangeEnd = 789787, XrefRangeStart = 789771, XrefRangeEnd = 789786, MetadataInitTokenRva = 46276020L, MetadataInitFlagRva = 59848194L)]
	public unsafe void GatherTopLevelBones()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GatherTopLevelBones_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789787, XrefRangeEnd = 789793, MetadataInitTokenRva = 46276168L, MetadataInitFlagRva = 59848195L)]
	public unsafe void HandleUpdateLocal(ISkeletonAnimation animatedSkeletonComponent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animatedSkeletonComponent);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HandleUpdateLocal_Private_Void_ISkeletonAnimation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789793, XrefRangeEnd = 789794, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AdjustMovementDeltaToConfiguration(ref Vector2 localDelta, Skeleton skeleton)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = (nint)Unsafe.AsPointer(ref localDelta);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdjustMovementDeltaToConfiguration_Private_Void_byref_Vector2_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 789832, RefRangeEnd = 789833, XrefRangeStart = 789794, XrefRangeEnd = 789832, MetadataInitTokenRva = 46275932L, MetadataInitFlagRva = 59848196L)]
	public unsafe void ApplyRootMotion(Vector2 localDelta)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&localDelta);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyRootMotion_Private_Void_Vector2_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 789842, RefRangeEnd = 789844, XrefRangeStart = 789833, XrefRangeEnd = 789842, MetadataInitTokenRva = 46276320L, MetadataInitFlagRva = 59848197L)]
	public unsafe SkeletonRootMotionBase()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonRootMotionBase>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonRootMotionBase(IntPtr pointer)
		: base(pointer)
	{
	}
}
