using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class ClippingAttachment : VertexAttachment
{
	private static readonly IntPtr NativeFieldInfoPtr_endSlot;

	private static readonly IntPtr NativeMethodInfoPtr_set_EndSlot_Public_set_Void_SlotData_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0;

	public unsafe SlotData endSlot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_endSlot);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<SlotData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_endSlot)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slotData));
		}
	}

	public unsafe SlotData EndSlot
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 66744, RefRangeEnd = 66745, XrefRangeStart = 66744, XrefRangeEnd = 66745, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_EndSlot_Public_set_Void_SlotData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static ClippingAttachment()
	{
		Il2CppClassPointerStore<ClippingAttachment>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "ClippingAttachment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ClippingAttachment>.NativeClassPtr);
		NativeFieldInfoPtr_endSlot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClippingAttachment>.NativeClassPtr, "endSlot");
		NativeMethodInfoPtr_set_EndSlot_Public_set_Void_SlotData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClippingAttachment>.NativeClassPtr, 100663533);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClippingAttachment>.NativeClassPtr, 100663534);
		NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClippingAttachment>.NativeClassPtr, 100663535);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783415, RefRangeEnd = 783416, XrefRangeStart = 783411, XrefRangeEnd = 783415, MetadataInitTokenRva = 46421112L, MetadataInitFlagRva = 59849655L)]
	public unsafe ClippingAttachment(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ClippingAttachment>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783416, XrefRangeEnd = 783425, MetadataInitTokenRva = 46421088L, MetadataInitFlagRva = 59849656L)]
	public unsafe override Attachment Copy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
	}

	public ClippingAttachment(IntPtr pointer)
		: base(pointer)
	{
	}
}
