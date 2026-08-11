using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppSpine;

public class EventQueue : Il2CppSystem.Object
{
	public sealed class EventQueueEntry : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_type;

		private static readonly System.IntPtr NativeFieldInfoPtr_entry;

		private static readonly System.IntPtr NativeFieldInfoPtr_e;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_EventType_TrackEntry_Event_0;

		public unsafe EventType type
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_type);
				return *(EventType*)num;
			}
			set
			{
				*(EventType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_type)) = eventType;
			}
		}

		public unsafe TrackEntry entry
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_entry);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TrackEntry>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_entry)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntry));
			}
		}

		public unsafe Event e
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_e);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Event>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_e)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
			}
		}

		static EventQueueEntry()
		{
			Il2CppClassPointerStore<EventQueueEntry>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, "EventQueueEntry");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EventQueueEntry>.NativeClassPtr);
			NativeFieldInfoPtr_type = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventQueueEntry>.NativeClassPtr, "type");
			NativeFieldInfoPtr_entry = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventQueueEntry>.NativeClassPtr, "entry");
			NativeFieldInfoPtr_e = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventQueueEntry>.NativeClassPtr, "e");
			NativeMethodInfoPtr__ctor_Public_Void_EventType_TrackEntry_Event_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueueEntry>.NativeClassPtr, 100663477);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782921, XrefRangeEnd = 782923, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe EventQueueEntry(EventType eventType, TrackEntry trackEntry, Event e = null)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EventQueueEntry>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = (nint)(&eventType);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntry);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_EventType_TrackEntry_Event_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public EventQueueEntry(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public EventQueueEntry()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EventQueueEntry>.NativeClassPtr))
		{
		}
	}

	[OriginalName("spine-unity.dll", "", "EventType")]
	public enum EventType
	{
		Start,
		Interrupt,
		End,
		Dispose,
		Complete,
		Event
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_eventQueueEntries;

	private static readonly System.IntPtr NativeFieldInfoPtr_drainDisabled;

	private static readonly System.IntPtr NativeFieldInfoPtr_state;

	private static readonly System.IntPtr NativeFieldInfoPtr_trackEntryPool;

	private static readonly System.IntPtr NativeFieldInfoPtr_AnimationsChanged;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_AnimationsChanged_Internal_add_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_AnimationsChanged_Internal_rem_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Internal_Void_AnimationState_Action_Pool_1_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Start_Internal_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Interrupt_Internal_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_End_Internal_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Internal_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Complete_Internal_Void_TrackEntry_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Event_Internal_Void_TrackEntry_Event_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Drain_Internal_Void_0;

	public unsafe List<EventQueueEntry> eventQueueEntries
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_eventQueueEntries);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<EventQueueEntry>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_eventQueueEntries)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool drainDisabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_drainDisabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_drainDisabled)) = flag;
		}
	}

	public unsafe AnimationState state
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_state);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationState>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_state)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationState));
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

	public unsafe Il2CppSystem.Action AnimationsChanged
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AnimationsChanged);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_AnimationsChanged)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
		}
	}

	static EventQueue()
	{
		Il2CppClassPointerStore<EventQueue>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "EventQueue");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EventQueue>.NativeClassPtr);
		NativeFieldInfoPtr_eventQueueEntries = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, "eventQueueEntries");
		NativeFieldInfoPtr_drainDisabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, "drainDisabled");
		NativeFieldInfoPtr_state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, "state");
		NativeFieldInfoPtr_trackEntryPool = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, "trackEntryPool");
		NativeFieldInfoPtr_AnimationsChanged = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, "AnimationsChanged");
		NativeMethodInfoPtr_add_AnimationsChanged_Internal_add_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, 100663467);
		NativeMethodInfoPtr_remove_AnimationsChanged_Internal_rem_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, 100663468);
		NativeMethodInfoPtr__ctor_Internal_Void_AnimationState_Action_Pool_1_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, 100663469);
		NativeMethodInfoPtr_Start_Internal_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, 100663470);
		NativeMethodInfoPtr_Interrupt_Internal_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, 100663471);
		NativeMethodInfoPtr_End_Internal_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, 100663472);
		NativeMethodInfoPtr_Dispose_Internal_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, 100663473);
		NativeMethodInfoPtr_Complete_Internal_Void_TrackEntry_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, 100663474);
		NativeMethodInfoPtr_Event_Internal_Void_TrackEntry_Event_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, 100663475);
		NativeMethodInfoPtr_Drain_Internal_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventQueue>.NativeClassPtr, 100663476);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782923, XrefRangeEnd = 782927, MetadataInitTokenRva = 46334696L, MetadataInitFlagRva = 59849666L)]
	public unsafe void add_AnimationsChanged(Il2CppSystem.Action value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_AnimationsChanged_Internal_add_Void_Action_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782927, XrefRangeEnd = 782931, MetadataInitTokenRva = 46334736L, MetadataInitFlagRva = 59849667L)]
	public unsafe void remove_AnimationsChanged(Il2CppSystem.Action value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_AnimationsChanged_Internal_rem_Void_Action_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782931, XrefRangeEnd = 782945, MetadataInitTokenRva = 46334672L, MetadataInitFlagRva = 59849668L)]
	public unsafe EventQueue(AnimationState state, Il2CppSystem.Action HandleAnimationsChanged, Pool<TrackEntry> trackEntryPool)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EventQueue>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)state);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)HandleAnimationsChanged);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)trackEntryPool);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Internal_Void_AnimationState_Action_Pool_1_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782945, XrefRangeEnd = 782951, MetadataInitTokenRva = 46334624L, MetadataInitFlagRva = 59849669L)]
	public unsafe void Start(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Internal_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782956, RefRangeEnd = 782958, XrefRangeStart = 782951, XrefRangeEnd = 782956, MetadataInitTokenRva = 46334580L, MetadataInitFlagRva = 59849670L)]
	public unsafe void Interrupt(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Interrupt_Internal_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 782964, RefRangeEnd = 782972, XrefRangeStart = 782958, XrefRangeEnd = 782964, MetadataInitTokenRva = 46334496L, MetadataInitFlagRva = 59849671L)]
	public unsafe void End(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_End_Internal_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782972, XrefRangeEnd = 782977, MetadataInitTokenRva = 46334432L, MetadataInitFlagRva = 59849672L)]
	public unsafe void Dispose(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dispose_Internal_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782977, XrefRangeEnd = 782982, MetadataInitTokenRva = 46334392L, MetadataInitFlagRva = 59849673L)]
	public unsafe void Complete(TrackEntry entry)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Complete_Internal_Void_TrackEntry_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782987, RefRangeEnd = 782989, XrefRangeStart = 782982, XrefRangeEnd = 782987, MetadataInitTokenRva = 46334532L, MetadataInitFlagRva = 59849674L)]
	public unsafe void Event(TrackEntry entry, Event e)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)entry);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)e);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Event_Internal_Void_TrackEntry_Event_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 783012, RefRangeEnd = 783021, XrefRangeStart = 782989, XrefRangeEnd = 783012, MetadataInitTokenRva = 46334464L, MetadataInitFlagRva = 59849675L)]
	public unsafe void Drain()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Drain_Internal_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EventQueue(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
