using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SkeletonAnimation : SkeletonRenderer
{
	private static readonly IntPtr NativeFieldInfoPtr_state;

	private static readonly IntPtr NativeFieldInfoPtr_isUnscaleDeltaTime;

	private static readonly IntPtr NativeFieldInfoPtr_wasUpdatedAfterInit;

	private static readonly IntPtr NativeFieldInfoPtr__BeforeApply;

	private static readonly IntPtr NativeFieldInfoPtr__UpdateLocal;

	private static readonly IntPtr NativeFieldInfoPtr__UpdateWorld;

	private static readonly IntPtr NativeFieldInfoPtr__UpdateComplete;

	private static readonly IntPtr NativeFieldInfoPtr__animationName;

	private static readonly IntPtr NativeFieldInfoPtr_loop;

	private static readonly IntPtr NativeFieldInfoPtr_timeScale;

	private static readonly IntPtr NativeMethodInfoPtr_get_AnimationState_Public_Virtual_Final_New_get_AnimationState_0;

	private static readonly IntPtr NativeMethodInfoPtr_add__BeforeApply_Protected_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove__BeforeApply_Protected_rem_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_add__UpdateLocal_Protected_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove__UpdateLocal_Protected_rem_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_add__UpdateWorld_Protected_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove__UpdateWorld_Protected_rem_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_add__UpdateComplete_Protected_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove__UpdateComplete_Protected_rem_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_add_BeforeApply_Public_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove_BeforeApply_Public_rem_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_add_UpdateLocal_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove_UpdateLocal_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_add_UpdateWorld_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove_UpdateWorld_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_add_UpdateComplete_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_remove_UpdateComplete_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_AnimationName_Public_get_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_AnimationName_Public_set_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_AddToGameObject_Public_Static_SkeletonAnimation_GameObject_SkeletonDataAsset_0;

	private static readonly IntPtr NativeMethodInfoPtr_NewSkeletonAnimationGameObject_Public_Static_SkeletonAnimation_SkeletonDataAsset_0;

	private static readonly IntPtr NativeMethodInfoPtr_ClearState_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Initialize_Public_Virtual_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_ForceUpdate_Public_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_Update_Public_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_UpdateAnimationStatus_Protected_Void_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_ApplyAnimation_Protected_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_LateUpdate_Public_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe AnimationState state
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_state);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationState>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_state)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationState));
		}
	}

	public unsafe bool isUnscaleDeltaTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isUnscaleDeltaTime);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isUnscaleDeltaTime)) = flag;
		}
	}

	public unsafe bool wasUpdatedAfterInit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wasUpdatedAfterInit);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_wasUpdatedAfterInit)) = flag;
		}
	}

	public unsafe UpdateBonesDelegate _BeforeApply
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BeforeApply);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BeforeApply)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe UpdateBonesDelegate _UpdateLocal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateLocal);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateLocal)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe UpdateBonesDelegate _UpdateWorld
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateWorld);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateWorld)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe UpdateBonesDelegate _UpdateComplete
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateComplete);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateComplete)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe string _animationName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__animationName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__animationName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool loop
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_loop);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_loop)) = flag;
		}
	}

	public unsafe float timeScale
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeScale);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timeScale)) = num;
		}
	}

	public unsafe virtual AnimationState AnimationState
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AnimationState_Public_Virtual_Final_New_get_AnimationState_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<AnimationState>(intPtr) : null;
		}
	}

	public unsafe string AnimationName
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 789877, RefRangeEnd = 789879, XrefRangeStart = 789876, XrefRangeEnd = 789877, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AnimationName_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789879, XrefRangeEnd = 789886, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AnimationName_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static SkeletonAnimation()
	{
		Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonAnimation");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr);
		NativeFieldInfoPtr_state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, "state");
		NativeFieldInfoPtr_isUnscaleDeltaTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, "isUnscaleDeltaTime");
		NativeFieldInfoPtr_wasUpdatedAfterInit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, "wasUpdatedAfterInit");
		NativeFieldInfoPtr__BeforeApply = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, "_BeforeApply");
		NativeFieldInfoPtr__UpdateLocal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, "_UpdateLocal");
		NativeFieldInfoPtr__UpdateWorld = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, "_UpdateWorld");
		NativeFieldInfoPtr__UpdateComplete = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, "_UpdateComplete");
		NativeFieldInfoPtr__animationName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, "_animationName");
		NativeFieldInfoPtr_loop = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, "loop");
		NativeFieldInfoPtr_timeScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, "timeScale");
		NativeMethodInfoPtr_get_AnimationState_Public_Virtual_Final_New_get_AnimationState_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664009);
		NativeMethodInfoPtr_add__BeforeApply_Protected_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664010);
		NativeMethodInfoPtr_remove__BeforeApply_Protected_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664011);
		NativeMethodInfoPtr_add__UpdateLocal_Protected_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664012);
		NativeMethodInfoPtr_remove__UpdateLocal_Protected_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664013);
		NativeMethodInfoPtr_add__UpdateWorld_Protected_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664014);
		NativeMethodInfoPtr_remove__UpdateWorld_Protected_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664015);
		NativeMethodInfoPtr_add__UpdateComplete_Protected_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664016);
		NativeMethodInfoPtr_remove__UpdateComplete_Protected_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664017);
		NativeMethodInfoPtr_add_BeforeApply_Public_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664018);
		NativeMethodInfoPtr_remove_BeforeApply_Public_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664019);
		NativeMethodInfoPtr_add_UpdateLocal_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664020);
		NativeMethodInfoPtr_remove_UpdateLocal_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664021);
		NativeMethodInfoPtr_add_UpdateWorld_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664022);
		NativeMethodInfoPtr_remove_UpdateWorld_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664023);
		NativeMethodInfoPtr_add_UpdateComplete_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664024);
		NativeMethodInfoPtr_remove_UpdateComplete_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664025);
		NativeMethodInfoPtr_get_AnimationName_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664026);
		NativeMethodInfoPtr_set_AnimationName_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664027);
		NativeMethodInfoPtr_AddToGameObject_Public_Static_SkeletonAnimation_GameObject_SkeletonDataAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664028);
		NativeMethodInfoPtr_NewSkeletonAnimationGameObject_Public_Static_SkeletonAnimation_SkeletonDataAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664029);
		NativeMethodInfoPtr_ClearState_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664030);
		NativeMethodInfoPtr_Initialize_Public_Virtual_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664031);
		NativeMethodInfoPtr_Update_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664032);
		NativeMethodInfoPtr_ForceUpdate_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664033);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664034);
		NativeMethodInfoPtr_Update_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664035);
		NativeMethodInfoPtr_UpdateAnimationStatus_Protected_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664036);
		NativeMethodInfoPtr_ApplyAnimation_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664037);
		NativeMethodInfoPtr_LateUpdate_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664038);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr, 100664039);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789844, XrefRangeEnd = 789848, MetadataInitTokenRva = 46268716L, MetadataInitFlagRva = 59827209L)]
	public unsafe void add__BeforeApply(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add__BeforeApply_Protected_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789848, XrefRangeEnd = 789852, MetadataInitTokenRva = 46268864L, MetadataInitFlagRva = 59827210L)]
	public unsafe void remove__BeforeApply(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove__BeforeApply_Protected_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789852, XrefRangeEnd = 789856, MetadataInitTokenRva = 46268800L, MetadataInitFlagRva = 59827211L)]
	public unsafe void add__UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add__UpdateLocal_Protected_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789856, XrefRangeEnd = 789860, MetadataInitTokenRva = 46268976L, MetadataInitFlagRva = 59827212L)]
	public unsafe void remove__UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove__UpdateLocal_Protected_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789860, XrefRangeEnd = 789864, MetadataInitTokenRva = 46268860L, MetadataInitFlagRva = 59827213L)]
	public unsafe void add__UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add__UpdateWorld_Protected_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789864, XrefRangeEnd = 789868, MetadataInitTokenRva = 46268984L, MetadataInitFlagRva = 59827214L)]
	public unsafe void remove__UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove__UpdateWorld_Protected_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789868, XrefRangeEnd = 789872, MetadataInitTokenRva = 46268768L, MetadataInitFlagRva = 59827215L)]
	public unsafe void add__UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add__UpdateComplete_Protected_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789872, XrefRangeEnd = 789876, MetadataInitTokenRva = 46268928L, MetadataInitFlagRva = 59827216L)]
	public unsafe void remove__UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove__UpdateComplete_Protected_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46268716L, MetadataInitFlagRva = 59827209L)]
	public unsafe void add_BeforeApply(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_BeforeApply_Public_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46268864L, MetadataInitFlagRva = 59827210L)]
	public unsafe void remove_BeforeApply(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_BeforeApply_Public_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46268800L, MetadataInitFlagRva = 59827211L)]
	public unsafe virtual void add_UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_UpdateLocal_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46268976L, MetadataInitFlagRva = 59827212L)]
	public unsafe virtual void remove_UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_UpdateLocal_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46268860L, MetadataInitFlagRva = 59827213L)]
	public unsafe virtual void add_UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_UpdateWorld_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46268984L, MetadataInitFlagRva = 59827214L)]
	public unsafe virtual void remove_UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_UpdateWorld_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46268768L, MetadataInitFlagRva = 59827215L)]
	public unsafe virtual void add_UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_UpdateComplete_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46268928L, MetadataInitFlagRva = 59827216L)]
	public unsafe virtual void remove_UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_UpdateComplete_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789886, XrefRangeEnd = 789891, MetadataInitTokenRva = 46268544L, MetadataInitFlagRva = 59827217L)]
	public unsafe static SkeletonAnimation AddToGameObject(GameObject gameObject, SkeletonDataAsset skeletonDataAsset)
	{
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddToGameObject_Public_Static_SkeletonAnimation_GameObject_SkeletonDataAsset_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonAnimation>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789891, XrefRangeEnd = 789896, MetadataInitTokenRva = 46268596L, MetadataInitFlagRva = 59827218L)]
	public unsafe static SkeletonAnimation NewSkeletonAnimationGameObject(SkeletonDataAsset skeletonDataAsset)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewSkeletonAnimationGameObject_Public_Static_SkeletonAnimation_SkeletonDataAsset_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonAnimation>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789896, XrefRangeEnd = 789898, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void ClearState()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ClearState_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789898, XrefRangeEnd = 789909, MetadataInitTokenRva = 46268568L, MetadataInitFlagRva = 59827219L)]
	public unsafe override void Initialize(bool overwrite)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&overwrite);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Initialize_Public_Virtual_Void_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789909, XrefRangeEnd = 789914, MetadataInitTokenRva = 46268628L, MetadataInitFlagRva = 59827220L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 789915, RefRangeEnd = 789918, XrefRangeStart = 789914, XrefRangeEnd = 789915, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ForceUpdate(bool lateUpdate = false)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&lateUpdate);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ForceUpdate_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789918, XrefRangeEnd = 789919, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public new unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 789930, RefRangeEnd = 789938, XrefRangeStart = 789919, XrefRangeEnd = 789930, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update(float deltaTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&deltaTime);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789938, XrefRangeEnd = 789941, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateAnimationStatus(float deltaTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&deltaTime);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateAnimationStatus_Protected_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789941, XrefRangeEnd = 789950, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyAnimation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyAnimation_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789950, XrefRangeEnd = 789952, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_LateUpdate_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 789952, XrefRangeEnd = 789956, MetadataInitTokenRva = 46268668L, MetadataInitFlagRva = 59827221L)]
	public unsafe SkeletonAnimation()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonAnimation>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonAnimation(IntPtr pointer)
		: base(pointer)
	{
	}
}
