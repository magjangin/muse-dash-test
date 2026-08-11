using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class Event : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_data;

	private static readonly System.IntPtr NativeFieldInfoPtr_time;

	private static readonly System.IntPtr NativeFieldInfoPtr_intValue;

	private static readonly System.IntPtr NativeFieldInfoPtr_floatValue;

	private static readonly System.IntPtr NativeFieldInfoPtr_stringValue;

	private static readonly System.IntPtr NativeFieldInfoPtr_volume;

	private static readonly System.IntPtr NativeFieldInfoPtr_balance;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Data_Public_get_EventData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Time_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Int_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Float_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_String_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Single_EventData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	public unsafe EventData data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<EventData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eventData));
		}
	}

	public unsafe float time
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_time);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_time)) = num;
		}
	}

	public unsafe int intValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_intValue);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_intValue)) = num;
		}
	}

	public unsafe float floatValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_floatValue);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_floatValue)) = num;
		}
	}

	public unsafe string stringValue
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stringValue);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stringValue)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float volume
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_volume);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_volume)) = num;
		}
	}

	public unsafe float balance
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_balance);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_balance)) = num;
		}
	}

	public unsafe EventData Data
	{
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 51077, RefRangeEnd = 51088, XrefRangeStart = 51077, XrefRangeEnd = 51088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Data_Public_get_EventData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<EventData>(intPtr) : null;
		}
	}

	public unsafe float Time
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Time_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int Int
	{
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Int_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float Float
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 122209, RefRangeEnd = 122210, XrefRangeStart = 122209, XrefRangeEnd = 122210, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Float_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string String
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 63349, RefRangeEnd = 63352, XrefRangeStart = 63349, XrefRangeEnd = 63352, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_String_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static Event()
	{
		Il2CppClassPointerStore<Event>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "Event");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Event>.NativeClassPtr);
		NativeFieldInfoPtr_data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Event>.NativeClassPtr, "data");
		NativeFieldInfoPtr_time = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Event>.NativeClassPtr, "time");
		NativeFieldInfoPtr_intValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Event>.NativeClassPtr, "intValue");
		NativeFieldInfoPtr_floatValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Event>.NativeClassPtr, "floatValue");
		NativeFieldInfoPtr_stringValue = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Event>.NativeClassPtr, "stringValue");
		NativeFieldInfoPtr_volume = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Event>.NativeClassPtr, "volume");
		NativeFieldInfoPtr_balance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Event>.NativeClassPtr, "balance");
		NativeMethodInfoPtr_get_Data_Public_get_EventData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Event>.NativeClassPtr, 100663625);
		NativeMethodInfoPtr_get_Time_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Event>.NativeClassPtr, 100663626);
		NativeMethodInfoPtr_set_Int_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Event>.NativeClassPtr, 100663627);
		NativeMethodInfoPtr_set_Float_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Event>.NativeClassPtr, 100663628);
		NativeMethodInfoPtr_set_String_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Event>.NativeClassPtr, 100663629);
		NativeMethodInfoPtr__ctor_Public_Void_Single_EventData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Event>.NativeClassPtr, 100663630);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Event>.NativeClassPtr, 100663631);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 783734, RefRangeEnd = 783736, XrefRangeStart = 783724, XrefRangeEnd = 783734, MetadataInitTokenRva = 46339220L, MetadataInitFlagRva = 59849664L)]
	public unsafe Event(float time, EventData data)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Event>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&time);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Single_EventData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 64785, RefRangeEnd = 64788, XrefRangeStart = 64785, XrefRangeEnd = 64788, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public Event(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
