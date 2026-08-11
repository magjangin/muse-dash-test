using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppSpine;

public class AnimationState : Il2CppSystem.Object
{
	public sealed class TrackEntryDelegate : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_TrackEntry_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TrackEntry_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static TrackEntryDelegate()
		{
			Il2CppClassPointerStore<TrackEntryDelegate>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "TrackEntryDelegate");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntryDelegate>.NativeClassPtr, 100663441);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntryDelegate>.NativeClassPtr, 100663442);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TrackEntry_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntryDelegate>.NativeClassPtr, 100663443);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntryDelegate>.NativeClassPtr, 100663444);
		}

		[CallerCount(6192)]
		[CachedScanResults(RefRangeStart = 39733, RefRangeEnd = 45925, XrefRangeStart = 39733, XrefRangeEnd = 45925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe TrackEntryDelegate(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrackEntryDelegate>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(44)]
		[CachedScanResults(RefRangeStart = 87822, RefRangeEnd = 87866, XrefRangeStart = 87822, XrefRangeEnd = 87866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Invoke(TrackEntry trackEntry)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntry);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 47234, RefRangeEnd = 47236, XrefRangeStart = 47234, XrefRangeEnd = 47236, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(TrackEntry trackEntry, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntry);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TrackEntry_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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

		public TrackEntryDelegate(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator TrackEntryDelegate(System.Action<TrackEntry> P_0)
		{
			return DelegateSupport.ConvertDelegate<TrackEntryDelegate>((System.Delegate)P_0);
		}

		public static TrackEntryDelegate operator +(TrackEntryDelegate P_0, TrackEntryDelegate P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<TrackEntryDelegate>();
		}

		public static TrackEntryDelegate operator -(TrackEntryDelegate P_0, TrackEntryDelegate P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<TrackEntryDelegate>();
			}
			return (TrackEntryDelegate)obj;
		}
	}

	public sealed class TrackEntryEventDelegate : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_TrackEntry_Event_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TrackEntry_Event_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

		static TrackEntryEventDelegate()
		{
			Il2CppClassPointerStore<TrackEntryEventDelegate>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "TrackEntryEventDelegate");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntryEventDelegate>.NativeClassPtr, 100663445);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_TrackEntry_Event_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntryEventDelegate>.NativeClassPtr, 100663446);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TrackEntry_Event_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntryEventDelegate>.NativeClassPtr, 100663447);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntryEventDelegate>.NativeClassPtr, 100663448);
		}

		[CallerCount(6192)]
		[CachedScanResults(RefRangeStart = 39733, RefRangeEnd = 45925, XrefRangeStart = 39733, XrefRangeEnd = 45925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe TrackEntryEventDelegate(Il2CppSystem.Object @object, System.IntPtr method)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrackEntryEventDelegate>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(133)]
		[CachedScanResults(RefRangeStart = 46811, RefRangeEnd = 46944, XrefRangeStart = 46811, XrefRangeEnd = 46944, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void Invoke(TrackEntry trackEntry, Event e)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntry);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_TrackEntry_Event_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(TrackEntry trackEntry, Event e, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntry);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_TrackEntry_Event_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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

		public TrackEntryEventDelegate(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public static implicit operator TrackEntryEventDelegate(System.Action<TrackEntry, Event> P_0)
		{
			return DelegateSupport.ConvertDelegate<TrackEntryEventDelegate>((System.Delegate)P_0);
		}

		public static TrackEntryEventDelegate operator +(TrackEntryEventDelegate P_0, TrackEntryEventDelegate P_1)
		{
			return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<TrackEntryEventDelegate>();
		}

		public static TrackEntryEventDelegate operator -(TrackEntryEventDelegate P_0, TrackEntryEventDelegate P_1)
		{
			object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
			if (obj != null)
			{
				obj = ((Il2CppObjectBase)obj).Cast<TrackEntryEventDelegate>();
			}
			return (TrackEntryEventDelegate)obj;
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_EmptyAnimation;

	private static readonly System.IntPtr NativeFieldInfoPtr_Subsequent;

	private static readonly System.IntPtr NativeFieldInfoPtr_First;

	private static readonly System.IntPtr NativeFieldInfoPtr_HoldSubsequent;

	private static readonly System.IntPtr NativeFieldInfoPtr_HoldFirst;

	private static readonly System.IntPtr NativeFieldInfoPtr_HoldMix;

	private static readonly System.IntPtr NativeFieldInfoPtr_Setup;

	private static readonly System.IntPtr NativeFieldInfoPtr_Current;

	private static readonly System.IntPtr NativeFieldInfoPtr_data;

	private static readonly System.IntPtr NativeFieldInfoPtr_tracks;

	private static readonly System.IntPtr NativeFieldInfoPtr_events;

	private static readonly System.IntPtr NativeFieldInfoPtr_Start;

	private static readonly System.IntPtr NativeFieldInfoPtr_Interrupt;

	private static readonly System.IntPtr NativeFieldInfoPtr_End;

	private static readonly System.IntPtr NativeFieldInfoPtr_Dispose;

	private static readonly System.IntPtr NativeFieldInfoPtr_Complete;

	private static readonly System.IntPtr NativeFieldInfoPtr_Event;

	private static readonly System.IntPtr NativeFieldInfoPtr_noApplyMixFrom;

	private static readonly System.IntPtr NativeFieldInfoPtr_queue;

	private static readonly System.IntPtr NativeFieldInfoPtr_propertyIDs;

	private static readonly System.IntPtr NativeFieldInfoPtr_animationsChanged;

	private static readonly System.IntPtr NativeFieldInfoPtr_timeScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_unkeyedState;

	private static readonly System.IntPtr NativeFieldInfoPtr_trackEntryPool;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnStart_Internal_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnInterrupt_Internal_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnd_Internal_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDispose_Internal_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnComplete_Internal_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEvent_Internal_Void_TrackEntry_Event_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_Event_Public_add_Void_TrackEntryEventDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_Event_Public_rem_Void_TrackEntryEventDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_AnimationStateData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Public_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateMixingFrom_Private_Boolean_TrackEntry_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Apply_Public_Boolean_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyEventTimelinesOnly_Public_Boolean_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyMixingFrom_Private_Single_TrackEntry_Skeleton_MixBlend_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyMixingFromEventTimelinesOnly_Private_Single_TrackEntry_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyAttachmentTimeline_Private_Void_AttachmentTimeline_Skeleton_Single_MixBlend_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetAttachment_Private_Void_Skeleton_Slot_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyRotateTimeline_Private_Static_Void_RotateTimeline_Skeleton_Single_Single_MixBlend_Il2CppStructArray_1_Single_Int32_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_QueueEvents_Private_Void_TrackEntry_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearTracks_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearTrack_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetCurrent_Private_Void_Int32_TrackEntry_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetAnimation_Public_TrackEntry_Int32_String_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetAnimation_Public_TrackEntry_Int32_Animation_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddAnimation_Public_TrackEntry_Int32_String_Boolean_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddAnimation_Public_TrackEntry_Int32_Animation_Boolean_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetEmptyAnimation_Public_TrackEntry_Int32_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetEmptyAnimations_Public_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ExpandToIndex_Private_TrackEntry_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NewTrackEntry_Private_TrackEntry_Int32_Animation_Boolean_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DisposeNext_Private_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AnimationsChanged_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ComputeHold_Private_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCurrent_Public_TrackEntry_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_TimeScale_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Data_Public_get_AnimationStateData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Tracks_Public_get_ExposedList_1_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr___ctor_b__46_0_Private_Void_0;

	public unsafe static Animation EmptyAnimation
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_EmptyAnimation, (void*)(&intPtr));
			System.IntPtr intPtr2 = intPtr;
			return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr2) : null;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_EmptyAnimation, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation));
		}
	}

	public unsafe static int Subsequent
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Subsequent, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Subsequent, (void*)(&num));
		}
	}

	public unsafe static int First
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_First, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_First, (void*)(&num));
		}
	}

	public unsafe static int HoldSubsequent
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HoldSubsequent, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HoldSubsequent, (void*)(&num));
		}
	}

	public unsafe static int HoldFirst
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HoldFirst, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HoldFirst, (void*)(&num));
		}
	}

	public unsafe static int HoldMix
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_HoldMix, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_HoldMix, (void*)(&num));
		}
	}

	public unsafe static int Setup
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Setup, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Setup, (void*)(&num));
		}
	}

	public unsafe static int Current
	{
		get
		{
			Unsafe.SkipInit(out int result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Current, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Current, (void*)(&num));
		}
	}

	public unsafe AnimationStateData data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationStateData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationStateData));
		}
	}

	public unsafe ExposedList<TrackEntry> tracks
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tracks);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<TrackEntry>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tracks)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<Event> events
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_events);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<Event>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_events)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe TrackEntryDelegate Start
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Start);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntryDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Start)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryDelegate));
		}
	}

	public unsafe TrackEntryDelegate Interrupt
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Interrupt);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntryDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Interrupt)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryDelegate));
		}
	}

	public unsafe TrackEntryDelegate End
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_End);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntryDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_End)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryDelegate));
		}
	}

	public unsafe TrackEntryDelegate Dispose
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Dispose);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntryDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Dispose)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryDelegate));
		}
	}

	public unsafe TrackEntryDelegate Complete
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Complete);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntryDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Complete)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryDelegate));
		}
	}

	public unsafe TrackEntryEventDelegate Event
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Event);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntryEventDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Event)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryEventDelegate));
		}
	}

	public unsafe bool noApplyMixFrom
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_noApplyMixFrom);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_noApplyMixFrom)) = flag;
		}
	}

	public unsafe EventQueue queue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_queue);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<EventQueue>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_queue)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eventQueue));
		}
	}

	public unsafe HashSet<int> propertyIDs
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_propertyIDs);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_propertyIDs)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hashSet));
		}
	}

	public unsafe bool animationsChanged
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationsChanged);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationsChanged)) = flag;
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

	public unsafe int unkeyedState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_unkeyedState);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_unkeyedState)) = num;
		}
	}

	public unsafe Pool<TrackEntry> trackEntryPool
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_trackEntryPool);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Pool<TrackEntry>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_trackEntryPool)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pool));
		}
	}

	public unsafe float TimeScale
	{
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_TimeScale_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe AnimationStateData Data
	{
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 51077, RefRangeEnd = 51088, XrefRangeStart = 51077, XrefRangeEnd = 51088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Data_Public_get_AnimationStateData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationStateData>(intPtr) : null;
		}
	}

	public unsafe ExposedList<TrackEntry> Tracks
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Tracks_Public_get_ExposedList_1_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<TrackEntry>>(intPtr) : null;
		}
	}

	static AnimationState()
	{
		Il2CppClassPointerStore<AnimationState>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "AnimationState");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AnimationState>.NativeClassPtr);
		NativeFieldInfoPtr_EmptyAnimation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "EmptyAnimation");
		NativeFieldInfoPtr_Subsequent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "Subsequent");
		NativeFieldInfoPtr_First = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "First");
		NativeFieldInfoPtr_HoldSubsequent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "HoldSubsequent");
		NativeFieldInfoPtr_HoldFirst = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "HoldFirst");
		NativeFieldInfoPtr_HoldMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "HoldMix");
		NativeFieldInfoPtr_Setup = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "Setup");
		NativeFieldInfoPtr_Current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "Current");
		NativeFieldInfoPtr_data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "data");
		NativeFieldInfoPtr_tracks = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "tracks");
		NativeFieldInfoPtr_events = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "events");
		NativeFieldInfoPtr_Start = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "Start");
		NativeFieldInfoPtr_Interrupt = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "Interrupt");
		NativeFieldInfoPtr_End = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "End");
		NativeFieldInfoPtr_Dispose = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "Dispose");
		NativeFieldInfoPtr_Complete = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "Complete");
		NativeFieldInfoPtr_Event = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "Event");
		NativeFieldInfoPtr_noApplyMixFrom = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "noApplyMixFrom");
		NativeFieldInfoPtr_queue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "queue");
		NativeFieldInfoPtr_propertyIDs = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "propertyIDs");
		NativeFieldInfoPtr_animationsChanged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "animationsChanged");
		NativeFieldInfoPtr_timeScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "timeScale");
		NativeFieldInfoPtr_unkeyedState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "unkeyedState");
		NativeFieldInfoPtr_trackEntryPool = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, "trackEntryPool");
		NativeMethodInfoPtr_OnStart_Internal_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663401);
		NativeMethodInfoPtr_OnInterrupt_Internal_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663402);
		NativeMethodInfoPtr_OnEnd_Internal_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663403);
		NativeMethodInfoPtr_OnDispose_Internal_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663404);
		NativeMethodInfoPtr_OnComplete_Internal_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663405);
		NativeMethodInfoPtr_OnEvent_Internal_Void_TrackEntry_Event_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663406);
		NativeMethodInfoPtr_add_Event_Public_add_Void_TrackEntryEventDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663407);
		NativeMethodInfoPtr_remove_Event_Public_rem_Void_TrackEntryEventDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663408);
		NativeMethodInfoPtr__ctor_Public_Void_AnimationStateData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663409);
		NativeMethodInfoPtr_Update_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663410);
		NativeMethodInfoPtr_UpdateMixingFrom_Private_Boolean_TrackEntry_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663411);
		NativeMethodInfoPtr_Apply_Public_Boolean_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663412);
		NativeMethodInfoPtr_ApplyEventTimelinesOnly_Public_Boolean_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663413);
		NativeMethodInfoPtr_ApplyMixingFrom_Private_Single_TrackEntry_Skeleton_MixBlend_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663414);
		NativeMethodInfoPtr_ApplyMixingFromEventTimelinesOnly_Private_Single_TrackEntry_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663415);
		NativeMethodInfoPtr_ApplyAttachmentTimeline_Private_Void_AttachmentTimeline_Skeleton_Single_MixBlend_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663416);
		NativeMethodInfoPtr_SetAttachment_Private_Void_Skeleton_Slot_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663417);
		NativeMethodInfoPtr_ApplyRotateTimeline_Private_Static_Void_RotateTimeline_Skeleton_Single_Single_MixBlend_Il2CppStructArray_1_Single_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663418);
		NativeMethodInfoPtr_QueueEvents_Private_Void_TrackEntry_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663419);
		NativeMethodInfoPtr_ClearTracks_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663420);
		NativeMethodInfoPtr_ClearTrack_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663421);
		NativeMethodInfoPtr_SetCurrent_Private_Void_Int32_TrackEntry_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663422);
		NativeMethodInfoPtr_SetAnimation_Public_TrackEntry_Int32_String_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663423);
		NativeMethodInfoPtr_SetAnimation_Public_TrackEntry_Int32_Animation_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663424);
		NativeMethodInfoPtr_AddAnimation_Public_TrackEntry_Int32_String_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663425);
		NativeMethodInfoPtr_AddAnimation_Public_TrackEntry_Int32_Animation_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663426);
		NativeMethodInfoPtr_SetEmptyAnimation_Public_TrackEntry_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663427);
		NativeMethodInfoPtr_SetEmptyAnimations_Public_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663428);
		NativeMethodInfoPtr_ExpandToIndex_Private_TrackEntry_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663429);
		NativeMethodInfoPtr_NewTrackEntry_Private_TrackEntry_Int32_Animation_Boolean_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663430);
		NativeMethodInfoPtr_DisposeNext_Private_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663431);
		NativeMethodInfoPtr_AnimationsChanged_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663432);
		NativeMethodInfoPtr_ComputeHold_Private_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663433);
		NativeMethodInfoPtr_GetCurrent_Public_TrackEntry_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663434);
		NativeMethodInfoPtr_set_TimeScale_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663435);
		NativeMethodInfoPtr_get_Data_Public_get_AnimationStateData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663436);
		NativeMethodInfoPtr_get_Tracks_Public_get_ExposedList_1_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663437);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663438);
		NativeMethodInfoPtr___ctor_b__46_0_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationState>.NativeClassPtr, 100663440);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782285, XrefRangeEnd = 782286, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnStart(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnStart_Internal_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782286, XrefRangeEnd = 782287, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnInterrupt(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnInterrupt_Internal_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782287, XrefRangeEnd = 782288, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEnd(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnd_Internal_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782288, XrefRangeEnd = 782289, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDispose(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDispose_Internal_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782289, XrefRangeEnd = 782290, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnComplete(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnComplete_Internal_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782290, XrefRangeEnd = 782291, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEvent(TrackEntry entry, Event e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEvent_Internal_Void_TrackEntry_Event_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 782295, RefRangeEnd = 782298, XrefRangeStart = 782291, XrefRangeEnd = 782295, MetadataInitTokenRva = 47222676L, MetadataInitFlagRva = 59849601L)]
	public unsafe void add_Event(TrackEntryEventDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_Event_Public_add_Void_TrackEntryEventDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 782302, RefRangeEnd = 782305, XrefRangeStart = 782298, XrefRangeEnd = 782302, MetadataInitTokenRva = 47222688L, MetadataInitFlagRva = 59849602L)]
	public unsafe void remove_Event(TrackEntryEventDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_Event_Public_rem_Void_TrackEntryEventDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782356, RefRangeEnd = 782358, XrefRangeStart = 782305, XrefRangeEnd = 782356, MetadataInitTokenRva = 47222636L, MetadataInitFlagRva = 59849603L)]
	public unsafe AnimationState(AnimationStateData data)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AnimationState>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_AnimationStateData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 782386, RefRangeEnd = 782390, XrefRangeStart = 782358, XrefRangeEnd = 782386, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update(float delta)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&delta);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782394, RefRangeEnd = 782396, XrefRangeStart = 782390, XrefRangeEnd = 782394, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool UpdateMixingFrom(TrackEntry to, float delta)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)to);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &delta;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateMixingFrom_Private_Boolean_TrackEntry_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 782436, RefRangeEnd = 782442, XrefRangeStart = 782396, XrefRangeEnd = 782436, MetadataInitTokenRva = 47222184L, MetadataInitFlagRva = 59849604L)]
	public unsafe bool Apply(Skeleton skeleton)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Apply_Public_Boolean_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 782454, RefRangeEnd = 782458, XrefRangeStart = 782442, XrefRangeEnd = 782454, MetadataInitTokenRva = 47222028L, MetadataInitFlagRva = 59849605L)]
	public unsafe bool ApplyEventTimelinesOnly(Skeleton skeleton)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyEventTimelinesOnly_Public_Boolean_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782494, RefRangeEnd = 782496, XrefRangeStart = 782458, XrefRangeEnd = 782494, MetadataInitTokenRva = 47222096L, MetadataInitFlagRva = 59849606L)]
	public unsafe float ApplyMixingFrom(TrackEntry to, Skeleton skeleton, MixBlend blend)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)to);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(MixBlend**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &blend;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyMixingFrom_Private_Single_TrackEntry_Skeleton_MixBlend_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782509, RefRangeEnd = 782511, XrefRangeStart = 782496, XrefRangeEnd = 782509, MetadataInitTokenRva = 47222064L, MetadataInitFlagRva = 59849607L)]
	public unsafe float ApplyMixingFromEventTimelinesOnly(TrackEntry to, Skeleton skeleton)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)to);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyMixingFromEventTimelinesOnly_Private_Single_TrackEntry_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 782521, RefRangeEnd = 782524, XrefRangeStart = 782511, XrefRangeEnd = 782521, MetadataInitTokenRva = 46278060L, MetadataInitFlagRva = 59849728L)]
	public unsafe void ApplyAttachmentTimeline(AttachmentTimeline timeline, Skeleton skeleton, float time, MixBlend blend, bool attachments)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)timeline);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &time;
		*(MixBlend**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &blend;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &attachments;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyAttachmentTimeline_Private_Void_AttachmentTimeline_Skeleton_Single_MixBlend_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782524, XrefRangeEnd = 782529, MetadataInitTokenRva = 46278060L, MetadataInitFlagRva = 59849728L)]
	public unsafe void SetAttachment(Skeleton skeleton, Slot slot, string attachmentName, bool attachments)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attachmentName);
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &attachments;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetAttachment_Private_Void_Skeleton_Slot_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782530, RefRangeEnd = 782532, XrefRangeStart = 782529, XrefRangeEnd = 782530, MetadataInitTokenRva = 47222148L, MetadataInitFlagRva = 59849608L)]
	public unsafe static void ApplyRotateTimeline(RotateTimeline timeline, Skeleton skeleton, float time, float alpha, MixBlend blend, Il2CppStructArray<float> timelinesRotation, int i, bool firstFrame)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[8];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)timeline);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &time;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &alpha;
		*(MixBlend**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &blend;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)timelinesRotation);
		*(int**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &i;
		*(bool**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &firstFrame;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyRotateTimeline_Private_Static_Void_RotateTimeline_Skeleton_Single_Single_MixBlend_Il2CppStructArray_1_Single_Int32_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 782544, RefRangeEnd = 782548, XrefRangeStart = 782532, XrefRangeEnd = 782544, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void QueueEvents(TrackEntry entry, float animationTime)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &animationTime;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_QueueEvents_Private_Void_TrackEntry_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 782565, RefRangeEnd = 782575, XrefRangeStart = 782548, XrefRangeEnd = 782565, MetadataInitTokenRva = 47222240L, MetadataInitFlagRva = 59849609L)]
	public unsafe void ClearTracks()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearTracks_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 782583, RefRangeEnd = 782588, XrefRangeStart = 782575, XrefRangeEnd = 782583, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearTrack(int trackIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&trackIndex);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearTrack_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 782606, RefRangeEnd = 782609, XrefRangeStart = 782588, XrefRangeEnd = 782606, MetadataInitTokenRva = 47222448L, MetadataInitFlagRva = 59849610L)]
	public unsafe void SetCurrent(int index, TrackEntry current, bool interrupt)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&index);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)current);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &interrupt;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCurrent_Private_Void_Int32_TrackEntry_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(60)]
	[CachedScanResults(RefRangeStart = 782621, RefRangeEnd = 782681, XrefRangeStart = 782609, XrefRangeEnd = 782621, MetadataInitTokenRva = 47222404L, MetadataInitFlagRva = 59849611L)]
	public unsafe TrackEntry SetAnimation(int trackIndex, string animationName, bool loop)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&trackIndex);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(animationName);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loop;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetAnimation_Public_TrackEntry_Int32_String_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 782693, RefRangeEnd = 782699, XrefRangeStart = 782681, XrefRangeEnd = 782693, MetadataInitTokenRva = 47222424L, MetadataInitFlagRva = 59849612L)]
	public unsafe TrackEntry SetAnimation(int trackIndex, Animation animation, bool loop)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&trackIndex);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loop;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetAnimation_Public_TrackEntry_Int32_Animation_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
	}

	[CallerCount(10)]
	[CachedScanResults(RefRangeStart = 782711, RefRangeEnd = 782721, XrefRangeStart = 782699, XrefRangeEnd = 782711, MetadataInitTokenRva = 47221948L, MetadataInitFlagRva = 59849613L)]
	public unsafe TrackEntry AddAnimation(int trackIndex, string animationName, bool loop, float delay)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&trackIndex);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(animationName);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loop;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &delay;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddAnimation_Public_TrackEntry_Int32_String_Boolean_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 782740, RefRangeEnd = 782741, XrefRangeStart = 782721, XrefRangeEnd = 782740, MetadataInitTokenRva = 47221896L, MetadataInitFlagRva = 59849614L)]
	public unsafe TrackEntry AddAnimation(int trackIndex, Animation animation, bool loop, float delay)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&trackIndex);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loop;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &delay;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddAnimation_Public_TrackEntry_Int32_Animation_Boolean_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 782746, RefRangeEnd = 782747, XrefRangeStart = 782741, XrefRangeEnd = 782746, MetadataInitTokenRva = 47222508L, MetadataInitFlagRva = 59849615L)]
	public unsafe TrackEntry SetEmptyAnimation(int trackIndex, float mixDuration)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&trackIndex);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &mixDuration;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetEmptyAnimation_Public_TrackEntry_Int32_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 782752, RefRangeEnd = 782757, XrefRangeStart = 782747, XrefRangeEnd = 782752, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetEmptyAnimations(float mixDuration)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&mixDuration);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetEmptyAnimations_Public_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 782760, RefRangeEnd = 782763, XrefRangeStart = 782757, XrefRangeEnd = 782760, MetadataInitTokenRva = 47222320L, MetadataInitFlagRva = 59849616L)]
	public unsafe TrackEntry ExpandToIndex(int index)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ExpandToIndex_Private_TrackEntry_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782774, RefRangeEnd = 782776, XrefRangeStart = 782763, XrefRangeEnd = 782774, MetadataInitTokenRva = 47222336L, MetadataInitFlagRva = 59849617L)]
	public unsafe TrackEntry NewTrackEntry(int trackIndex, Animation animation, bool loop, TrackEntry last)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = (nint)(&trackIndex);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loop;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)last);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NewTrackEntry_Private_TrackEntry_Int32_Animation_Boolean_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 782784, RefRangeEnd = 782789, XrefRangeStart = 782776, XrefRangeEnd = 782784, MetadataInitTokenRva = 46334432L, MetadataInitFlagRva = 59849672L)]
	public unsafe void DisposeNext(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DisposeNext_Private_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782789, XrefRangeEnd = 782794, MetadataInitTokenRva = 47222004L, MetadataInitFlagRva = 59849618L)]
	public unsafe void AnimationsChanged()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AnimationsChanged_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782805, RefRangeEnd = 782807, XrefRangeStart = 782794, XrefRangeEnd = 782805, MetadataInitTokenRva = 47222256L, MetadataInitFlagRva = 59849619L)]
	public unsafe void ComputeHold(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ComputeHold_Private_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(18)]
	[CachedScanResults(RefRangeStart = 782807, RefRangeEnd = 782825, XrefRangeStart = 782807, XrefRangeEnd = 782807, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe TrackEntry GetCurrent(int trackIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&trackIndex);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCurrent_Public_TrackEntry_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782825, XrefRangeEnd = 782834, MetadataInitTokenRva = 47222556L, MetadataInitFlagRva = 59849620L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	public unsafe void __ctor_b__46_0()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr___ctor_b__46_0_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AnimationState(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
