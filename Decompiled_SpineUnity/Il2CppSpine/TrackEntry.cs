using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class TrackEntry : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_animation;

	private static readonly System.IntPtr NativeFieldInfoPtr_next;

	private static readonly System.IntPtr NativeFieldInfoPtr_mixingFrom;

	private static readonly System.IntPtr NativeFieldInfoPtr_mixingTo;

	private static readonly System.IntPtr NativeFieldInfoPtr_Start;

	private static readonly System.IntPtr NativeFieldInfoPtr_Interrupt;

	private static readonly System.IntPtr NativeFieldInfoPtr_End;

	private static readonly System.IntPtr NativeFieldInfoPtr_Dispose;

	private static readonly System.IntPtr NativeFieldInfoPtr_Complete;

	private static readonly System.IntPtr NativeFieldInfoPtr_Event;

	private static readonly System.IntPtr NativeFieldInfoPtr_trackIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_loop;

	private static readonly System.IntPtr NativeFieldInfoPtr_holdPrevious;

	private static readonly System.IntPtr NativeFieldInfoPtr_eventThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_attachmentThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_drawOrderThreshold;

	private static readonly System.IntPtr NativeFieldInfoPtr_animationStart;

	private static readonly System.IntPtr NativeFieldInfoPtr_animationEnd;

	private static readonly System.IntPtr NativeFieldInfoPtr_animationLast;

	private static readonly System.IntPtr NativeFieldInfoPtr_nextAnimationLast;

	private static readonly System.IntPtr NativeFieldInfoPtr_delay;

	private static readonly System.IntPtr NativeFieldInfoPtr_trackTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_trackLast;

	private static readonly System.IntPtr NativeFieldInfoPtr_nextTrackLast;

	private static readonly System.IntPtr NativeFieldInfoPtr_trackEnd;

	private static readonly System.IntPtr NativeFieldInfoPtr_timeScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_alpha;

	private static readonly System.IntPtr NativeFieldInfoPtr_mixTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_mixDuration;

	private static readonly System.IntPtr NativeFieldInfoPtr_interruptAlpha;

	private static readonly System.IntPtr NativeFieldInfoPtr_totalAlpha;

	private static readonly System.IntPtr NativeFieldInfoPtr_mixBlend;

	private static readonly System.IntPtr NativeFieldInfoPtr_timelineMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_timelineHoldMix;

	private static readonly System.IntPtr NativeFieldInfoPtr_timelinesRotation;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_Complete_Public_add_Void_TrackEntryDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_Complete_Public_rem_Void_TrackEntryDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnStart_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnInterrupt_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnd_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDispose_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnComplete_Internal_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEvent_Internal_Void_Event_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Virtual_Final_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Animation_Public_get_Animation_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Loop_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_TrackTime_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_TrackTime_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AnimationEnd_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AnimationTime_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_IsComplete_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Animation animation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animation);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation));
		}
	}

	public unsafe TrackEntry next
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_next);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_next)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntry));
		}
	}

	public unsafe TrackEntry mixingFrom
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mixingFrom);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mixingFrom)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntry));
		}
	}

	public unsafe TrackEntry mixingTo
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mixingTo);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mixingTo)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntry));
		}
	}

	public unsafe AnimationState.TrackEntryDelegate Start
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Start);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationState.TrackEntryDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Start)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryDelegate));
		}
	}

	public unsafe AnimationState.TrackEntryDelegate Interrupt
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Interrupt);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationState.TrackEntryDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Interrupt)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryDelegate));
		}
	}

	public unsafe AnimationState.TrackEntryDelegate End
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_End);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationState.TrackEntryDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_End)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryDelegate));
		}
	}

	public unsafe AnimationState.TrackEntryDelegate Dispose
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Dispose);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationState.TrackEntryDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Dispose)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryDelegate));
		}
	}

	public unsafe AnimationState.TrackEntryDelegate Complete
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Complete);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationState.TrackEntryDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Complete)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryDelegate));
		}
	}

	public unsafe AnimationState.TrackEntryEventDelegate Event
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Event);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationState.TrackEntryEventDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Event)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryEventDelegate));
		}
	}

	public unsafe int trackIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_trackIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_trackIndex)) = num;
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

	public unsafe bool holdPrevious
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_holdPrevious);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_holdPrevious)) = flag;
		}
	}

	public unsafe float eventThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_eventThreshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_eventThreshold)) = num;
		}
	}

	public unsafe float attachmentThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentThreshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentThreshold)) = num;
		}
	}

	public unsafe float drawOrderThreshold
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_drawOrderThreshold);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_drawOrderThreshold)) = num;
		}
	}

	public unsafe float animationStart
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationStart);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationStart)) = num;
		}
	}

	public unsafe float animationEnd
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationEnd);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationEnd)) = num;
		}
	}

	public unsafe float animationLast
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationLast);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationLast)) = num;
		}
	}

	public unsafe float nextAnimationLast
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextAnimationLast);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextAnimationLast)) = num;
		}
	}

	public unsafe float delay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_delay);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_delay)) = num;
		}
	}

	public unsafe float trackTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_trackTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_trackTime)) = num;
		}
	}

	public unsafe float trackLast
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_trackLast);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_trackLast)) = num;
		}
	}

	public unsafe float nextTrackLast
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextTrackLast);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextTrackLast)) = num;
		}
	}

	public unsafe float trackEnd
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_trackEnd);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_trackEnd)) = num;
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

	public unsafe float alpha
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_alpha);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_alpha)) = num;
		}
	}

	public unsafe float mixTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mixTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mixTime)) = num;
		}
	}

	public unsafe float mixDuration
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mixDuration);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mixDuration)) = num;
		}
	}

	public unsafe float interruptAlpha
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interruptAlpha);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interruptAlpha)) = num;
		}
	}

	public unsafe float totalAlpha
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_totalAlpha);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_totalAlpha)) = num;
		}
	}

	public unsafe MixBlend mixBlend
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mixBlend);
			return *(MixBlend*)num;
		}
		set
		{
			*(MixBlend*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_mixBlend)) = mixBlend;
		}
	}

	public unsafe ExposedList<int> timelineMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timelineMode);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timelineMode)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<TrackEntry> timelineHoldMix
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timelineHoldMix);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<TrackEntry>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timelineHoldMix)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe ExposedList<float> timelinesRotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timelinesRotation);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_timelinesRotation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe Animation Animation
	{
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 51077, RefRangeEnd = 51088, XrefRangeStart = 51077, XrefRangeEnd = 51088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Animation_Public_get_Animation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
		}
	}

	public unsafe bool Loop
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Loop_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float TrackTime
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_TrackTime_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_TrackTime_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float AnimationEnd
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AnimationEnd_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe float AnimationTime
	{
		[CallerCount(7)]
		[CachedScanResults(RefRangeStart = 782894, RefRangeEnd = 782901, XrefRangeStart = 782890, XrefRangeEnd = 782894, MetadataInitTokenRva = 47208644L, MetadataInitFlagRva = 59867979L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AnimationTime_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool IsComplete
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 782901, RefRangeEnd = 782902, XrefRangeStart = 782901, XrefRangeEnd = 782901, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_IsComplete_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static TrackEntry()
	{
		Il2CppClassPointerStore<TrackEntry>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "TrackEntry");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr);
		NativeFieldInfoPtr_animation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "animation");
		NativeFieldInfoPtr_next = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "next");
		NativeFieldInfoPtr_mixingFrom = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "mixingFrom");
		NativeFieldInfoPtr_mixingTo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "mixingTo");
		NativeFieldInfoPtr_Start = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "Start");
		NativeFieldInfoPtr_Interrupt = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "Interrupt");
		NativeFieldInfoPtr_End = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "End");
		NativeFieldInfoPtr_Dispose = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "Dispose");
		NativeFieldInfoPtr_Complete = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "Complete");
		NativeFieldInfoPtr_Event = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "Event");
		NativeFieldInfoPtr_trackIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "trackIndex");
		NativeFieldInfoPtr_loop = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "loop");
		NativeFieldInfoPtr_holdPrevious = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "holdPrevious");
		NativeFieldInfoPtr_eventThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "eventThreshold");
		NativeFieldInfoPtr_attachmentThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "attachmentThreshold");
		NativeFieldInfoPtr_drawOrderThreshold = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "drawOrderThreshold");
		NativeFieldInfoPtr_animationStart = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "animationStart");
		NativeFieldInfoPtr_animationEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "animationEnd");
		NativeFieldInfoPtr_animationLast = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "animationLast");
		NativeFieldInfoPtr_nextAnimationLast = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "nextAnimationLast");
		NativeFieldInfoPtr_delay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "delay");
		NativeFieldInfoPtr_trackTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "trackTime");
		NativeFieldInfoPtr_trackLast = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "trackLast");
		NativeFieldInfoPtr_nextTrackLast = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "nextTrackLast");
		NativeFieldInfoPtr_trackEnd = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "trackEnd");
		NativeFieldInfoPtr_timeScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "timeScale");
		NativeFieldInfoPtr_alpha = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "alpha");
		NativeFieldInfoPtr_mixTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "mixTime");
		NativeFieldInfoPtr_mixDuration = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "mixDuration");
		NativeFieldInfoPtr_interruptAlpha = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "interruptAlpha");
		NativeFieldInfoPtr_totalAlpha = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "totalAlpha");
		NativeFieldInfoPtr_mixBlend = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "mixBlend");
		NativeFieldInfoPtr_timelineMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "timelineMode");
		NativeFieldInfoPtr_timelineHoldMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "timelineHoldMix");
		NativeFieldInfoPtr_timelinesRotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, "timelinesRotation");
		NativeMethodInfoPtr_add_Complete_Public_add_Void_TrackEntryDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663449);
		NativeMethodInfoPtr_remove_Complete_Public_rem_Void_TrackEntryDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663450);
		NativeMethodInfoPtr_OnStart_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663451);
		NativeMethodInfoPtr_OnInterrupt_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663452);
		NativeMethodInfoPtr_OnEnd_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663453);
		NativeMethodInfoPtr_OnDispose_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663454);
		NativeMethodInfoPtr_OnComplete_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663455);
		NativeMethodInfoPtr_OnEvent_Internal_Void_Event_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663456);
		NativeMethodInfoPtr_Reset_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663457);
		NativeMethodInfoPtr_get_Animation_Public_get_Animation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663458);
		NativeMethodInfoPtr_get_Loop_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663459);
		NativeMethodInfoPtr_get_TrackTime_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663460);
		NativeMethodInfoPtr_set_TrackTime_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663461);
		NativeMethodInfoPtr_get_AnimationEnd_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663462);
		NativeMethodInfoPtr_get_AnimationTime_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663463);
		NativeMethodInfoPtr_get_IsComplete_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663464);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663465);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr, 100663466);
	}

	[SpecialName]
	[CallerCount(18)]
	[CachedScanResults(RefRangeStart = 782838, RefRangeEnd = 782856, XrefRangeStart = 782834, XrefRangeEnd = 782838, MetadataInitTokenRva = 47208628L, MetadataInitFlagRva = 59867976L)]
	public unsafe void add_Complete(AnimationState.TrackEntryDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_Complete_Public_add_Void_TrackEntryDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782856, XrefRangeEnd = 782860, MetadataInitTokenRva = 47208688L, MetadataInitFlagRva = 59867977L)]
	public unsafe void remove_Complete(AnimationState.TrackEntryDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_Complete_Public_rem_Void_TrackEntryDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 782861, RefRangeEnd = 782862, XrefRangeStart = 782860, XrefRangeEnd = 782861, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnStart()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnStart_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 782863, RefRangeEnd = 782864, XrefRangeStart = 782862, XrefRangeEnd = 782863, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnInterrupt()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnInterrupt_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 782865, RefRangeEnd = 782866, XrefRangeStart = 782864, XrefRangeEnd = 782865, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEnd()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEnd_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 782867, RefRangeEnd = 782868, XrefRangeStart = 782866, XrefRangeEnd = 782867, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDispose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDispose_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 782869, RefRangeEnd = 782870, XrefRangeStart = 782868, XrefRangeEnd = 782869, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnComplete()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnComplete_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 782871, RefRangeEnd = 782872, XrefRangeStart = 782870, XrefRangeEnd = 782871, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnEvent(Event e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnEvent_Internal_Void_Event_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782872, XrefRangeEnd = 782890, MetadataInitTokenRva = 47208512L, MetadataInitFlagRva = 59867978L)]
	public unsafe virtual void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782902, XrefRangeEnd = 782904, MetadataInitTokenRva = 47208520L, MetadataInitFlagRva = 59867980L)]
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
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782904, XrefRangeEnd = 782921, MetadataInitTokenRva = 47208564L, MetadataInitFlagRva = 59867981L)]
	public unsafe TrackEntry()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TrackEntry>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TrackEntry(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
