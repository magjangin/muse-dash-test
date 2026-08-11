using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class PathConstraintPositionTimeline : CurveTimeline
{
	private static readonly IntPtr NativeFieldInfoPtr_pathConstraintIndex;

	private static readonly IntPtr NativeFieldInfoPtr_frames;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_PropertyId_Public_Virtual_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Apply_Public_Virtual_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0;

	public unsafe int pathConstraintIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pathConstraintIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_pathConstraintIndex)) = num;
		}
	}

	public unsafe Il2CppStructArray<float> frames
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frames);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frames)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe override int PropertyId
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_PropertyId_Public_Virtual_get_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static PathConstraintPositionTimeline()
	{
		Il2CppClassPointerStore<PathConstraintPositionTimeline>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "PathConstraintPositionTimeline");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<PathConstraintPositionTimeline>.NativeClassPtr);
		NativeFieldInfoPtr_pathConstraintIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintPositionTimeline>.NativeClassPtr, "pathConstraintIndex");
		NativeFieldInfoPtr_frames = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<PathConstraintPositionTimeline>.NativeClassPtr, "frames");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraintPositionTimeline>.NativeClassPtr, 100663390);
		NativeMethodInfoPtr_get_PropertyId_Public_Virtual_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraintPositionTimeline>.NativeClassPtr, 100663391);
		NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraintPositionTimeline>.NativeClassPtr, 100663392);
		NativeMethodInfoPtr_Apply_Public_Virtual_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<PathConstraintPositionTimeline>.NativeClassPtr, 100663393);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 782265, RefRangeEnd = 782269, XrefRangeStart = 782260, XrefRangeEnd = 782265, MetadataInitTokenRva = 47162144L, MetadataInitFlagRva = 59849704L)]
	public unsafe PathConstraintPositionTimeline(int frameCount)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<PathConstraintPositionTimeline>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&frameCount);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 782065, RefRangeEnd = 782069, XrefRangeStart = 782065, XrefRangeEnd = 782069, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetFrame(int frameIndex, float time, float position)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&frameIndex);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &time;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &position;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782269, XrefRangeEnd = 782272, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &lastTime;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &time;
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)firedEvents);
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &alpha;
		*(MixBlend**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &blend;
		*(MixDirection**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &direction;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Apply_Public_Virtual_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public PathConstraintPositionTimeline(IntPtr pointer)
		: base(pointer)
	{
	}
}
