using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class PathAttachment : VertexAttachment
{
	private static readonly IntPtr NativeFieldInfoPtr_lengths;

	private static readonly IntPtr NativeFieldInfoPtr_closed;

	private static readonly IntPtr NativeFieldInfoPtr_constantSpeed;

	private static readonly IntPtr NativeMethodInfoPtr_get_Lengths_Public_get_Il2CppStructArray_1_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_ConstantSpeed_Public_get_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly IntPtr NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0;

	public unsafe Il2CppStructArray<float> lengths
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lengths);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_lengths)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool closed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_closed);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_closed)) = flag;
		}
	}

	public unsafe bool constantSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_constantSpeed);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_constantSpeed)) = flag;
		}
	}

	public unsafe Il2CppStructArray<float> Lengths
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 37591, RefRangeEnd = 37595, XrefRangeStart = 37591, XrefRangeEnd = 37595, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Lengths_Public_get_Il2CppStructArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
	}

	public unsafe bool Closed
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe bool ConstantSpeed
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_ConstantSpeed_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static PathAttachment()
	{
		Il2CppClassPointerStore<PathAttachment>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "PathAttachment");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathAttachment>.NativeClassPtr);
		NativeFieldInfoPtr_lengths = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathAttachment>.NativeClassPtr, "lengths");
		NativeFieldInfoPtr_closed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathAttachment>.NativeClassPtr, "closed");
		NativeFieldInfoPtr_constantSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathAttachment>.NativeClassPtr, "constantSpeed");
		NativeMethodInfoPtr_get_Lengths_Public_get_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathAttachment>.NativeClassPtr, 100663571);
		NativeMethodInfoPtr_get_Closed_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathAttachment>.NativeClassPtr, 100663572);
		NativeMethodInfoPtr_get_ConstantSpeed_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathAttachment>.NativeClassPtr, 100663573);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathAttachment>.NativeClassPtr, 100663574);
		NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathAttachment>.NativeClassPtr, 100663575);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 783493, RefRangeEnd = 783494, XrefRangeStart = 783489, XrefRangeEnd = 783493, MetadataInitTokenRva = 47162012L, MetadataInitFlagRva = 59849694L)]
	public unsafe PathAttachment(string name)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PathAttachment>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(name);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783494, XrefRangeEnd = 783506, MetadataInitTokenRva = 47161960L, MetadataInitFlagRva = 59849695L)]
	public unsafe override Attachment Copy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Copy_Public_Virtual_Attachment_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
	}

	public PathAttachment(IntPtr pointer)
		: base(pointer)
	{
	}
}
