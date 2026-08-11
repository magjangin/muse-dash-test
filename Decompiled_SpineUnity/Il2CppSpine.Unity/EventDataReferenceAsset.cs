using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class EventDataReferenceAsset : ScriptableObject
{
	private static readonly IntPtr NativeFieldInfoPtr_QuietSkeletonData;

	private static readonly IntPtr NativeFieldInfoPtr_skeletonDataAsset;

	private static readonly IntPtr NativeFieldInfoPtr_eventName;

	private static readonly IntPtr NativeFieldInfoPtr_eventData;

	private static readonly IntPtr NativeMethodInfoPtr_get_EventData_Public_get_EventData_0;

	private static readonly IntPtr NativeMethodInfoPtr_Initialize_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_op_Implicit_Public_Static_EventData_EventDataReferenceAsset_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe static bool QuietSkeletonData
	{
		get
		{
			Unsafe.SkipInit(out bool result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_QuietSkeletonData, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_QuietSkeletonData, (void*)(&flag));
		}
	}

	public unsafe SkeletonDataAsset skeletonDataAsset
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonDataAsset);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SkeletonDataAsset>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonDataAsset)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset));
		}
	}

	public unsafe string eventName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_eventName);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_eventName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe EventData eventData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_eventData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<EventData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_eventData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)eventData));
		}
	}

	public unsafe EventData EventData
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788240, XrefRangeEnd = 788241, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_EventData_Public_get_EventData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<EventData>(intPtr) : null;
		}
	}

	static EventDataReferenceAsset()
	{
		Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "EventDataReferenceAsset");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr);
		NativeFieldInfoPtr_QuietSkeletonData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr, "QuietSkeletonData");
		NativeFieldInfoPtr_skeletonDataAsset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr, "skeletonDataAsset");
		NativeFieldInfoPtr_eventName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr, "eventName");
		NativeFieldInfoPtr_eventData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr, "eventData");
		NativeMethodInfoPtr_get_EventData_Public_get_EventData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr, 100663862);
		NativeMethodInfoPtr_Initialize_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr, 100663863);
		NativeMethodInfoPtr_op_Implicit_Public_Static_EventData_EventDataReferenceAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr, 100663864);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr, 100663865);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 788259, RefRangeEnd = 788261, XrefRangeStart = 788241, XrefRangeEnd = 788259, MetadataInitTokenRva = 46329948L, MetadataInitFlagRva = 59827164L)]
	public unsafe void Initialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 788261, XrefRangeEnd = 788262, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static implicit operator EventData(EventDataReferenceAsset asset)
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)asset);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_op_Implicit_Public_Static_EventData_EventDataReferenceAsset_0, (IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<EventData>(intPtr) : null;
	}

	[CallerCount(31)]
	[CachedScanResults(RefRangeStart = 18721, RefRangeEnd = 18752, XrefRangeStart = 18721, XrefRangeEnd = 18752, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe EventDataReferenceAsset()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<EventDataReferenceAsset>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public EventDataReferenceAsset(IntPtr pointer)
		: base(pointer)
	{
	}
}
