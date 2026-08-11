using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;

namespace Il2CppSpine;

public class AnimationStateData : Il2CppSystem.Object
{
	public sealed class AnimationPair : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_a1;

		private static readonly System.IntPtr NativeFieldInfoPtr_a2;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Animation_Animation_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

		public unsafe Animation a1
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_a1);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_a1)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation));
			}
		}

		public unsafe Animation a2
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_a2);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_a2)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animation));
			}
		}

		static AnimationPair()
		{
			Il2CppClassPointerStore<AnimationPair>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr, "AnimationPair");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AnimationPair>.NativeClassPtr);
			NativeFieldInfoPtr_a1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationPair>.NativeClassPtr, "a1");
			NativeFieldInfoPtr_a2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationPair>.NativeClassPtr, "a2");
			NativeMethodInfoPtr__ctor_Public_Void_Animation_Animation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationPair>.NativeClassPtr, 100663490);
			NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationPair>.NativeClassPtr, 100663491);
		}

		[CallerCount(64)]
		[CachedScanResults(RefRangeStart = 201597, RefRangeEnd = 201661, XrefRangeStart = 201597, XrefRangeEnd = 201661, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe AnimationPair(Animation a1, Animation a2)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AnimationPair>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)a1);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)a2);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Animation_Animation_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783059, XrefRangeEnd = 783063, MetadataInitTokenRva = 47221276L, MetadataInitFlagRva = 59849626L)]
		public unsafe override string ToString()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}

		public AnimationPair(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public AnimationPair()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AnimationPair>.NativeClassPtr))
		{
		}
	}

	public class AnimationPairComparer : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_Instance;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_AnimationStateData_AnimationPair__Equals_Private_Virtual_Final_New_Boolean_AnimationPair_AnimationPair_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_AnimationStateData_AnimationPair__GetHashCode_Private_Virtual_Final_New_Int32_AnimationPair_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe static AnimationPairComparer Instance
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Instance, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<AnimationPairComparer>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Instance, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animationPairComparer));
			}
		}

		static AnimationPairComparer()
		{
			Il2CppClassPointerStore<AnimationPairComparer>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr, "AnimationPairComparer");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AnimationPairComparer>.NativeClassPtr);
			NativeFieldInfoPtr_Instance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationPairComparer>.NativeClassPtr, "Instance");
			NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_AnimationStateData_AnimationPair__Equals_Private_Virtual_Final_New_Boolean_AnimationPair_AnimationPair_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationPairComparer>.NativeClassPtr, 100663492);
			NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_AnimationStateData_AnimationPair__GetHashCode_Private_Virtual_Final_New_Int32_AnimationPair_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationPairComparer>.NativeClassPtr, 100663493);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationPairComparer>.NativeClassPtr, 100663494);
		}

		[CallerCount(0)]
		public unsafe virtual bool System_Collections_Generic_IEqualityComparer_Spine_AnimationStateData_AnimationPair__Equals(AnimationPair x, AnimationPair y)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)x));
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)y));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_AnimationStateData_AnimationPair__Equals_Private_Virtual_Final_New_Boolean_AnimationPair_AnimationPair_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe virtual int System_Collections_Generic_IEqualityComparer_Spine_AnimationStateData_AnimationPair__GetHashCode(AnimationPair obj)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)obj));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEqualityComparer_Spine_AnimationStateData_AnimationPair__GetHashCode_Private_Virtual_Final_New_Int32_AnimationPair_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(2392)]
		[CachedScanResults(RefRangeStart = 18875, RefRangeEnd = 21267, XrefRangeStart = 18875, XrefRangeEnd = 21267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe AnimationPairComparer()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AnimationPairComparer>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public AnimationPairComparer(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_skeletonData;

	private static readonly System.IntPtr NativeFieldInfoPtr_animationToMixTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_defaultMix;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_SkeletonData_Public_get_SkeletonData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_SkeletonData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMix_Public_Void_String_String_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMix_Public_Void_Animation_Animation_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetMix_Public_Single_Animation_Animation_0;

	public unsafe SkeletonData skeletonData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeletonData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData));
		}
	}

	public unsafe Dictionary<AnimationPair, float> animationToMixTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationToMixTime);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<AnimationPair, float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationToMixTime)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe float defaultMix
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultMix);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_defaultMix)) = num;
		}
	}

	public unsafe SkeletonData SkeletonData
	{
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 51077, RefRangeEnd = 51088, XrefRangeStart = 51077, XrefRangeEnd = 51088, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_SkeletonData_Public_get_SkeletonData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SkeletonData>(intPtr) : null;
		}
	}

	static AnimationStateData()
	{
		Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "AnimationStateData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr);
		NativeFieldInfoPtr_skeletonData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr, "skeletonData");
		NativeFieldInfoPtr_animationToMixTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr, "animationToMixTime");
		NativeFieldInfoPtr_defaultMix = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr, "defaultMix");
		NativeMethodInfoPtr_get_SkeletonData_Public_get_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr, 100663485);
		NativeMethodInfoPtr__ctor_Public_Void_SkeletonData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr, 100663486);
		NativeMethodInfoPtr_SetMix_Public_Void_String_String_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr, 100663487);
		NativeMethodInfoPtr_SetMix_Public_Void_Animation_Animation_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr, 100663488);
		NativeMethodInfoPtr_GetMix_Public_Single_Animation_Animation_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr, 100663489);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 783081, RefRangeEnd = 783083, XrefRangeStart = 783063, XrefRangeEnd = 783081, MetadataInitTokenRva = 47221856L, MetadataInitFlagRva = 59849622L)]
	public unsafe AnimationStateData(SkeletonData skeletonData)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AnimationStateData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_SkeletonData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 783093, RefRangeEnd = 783096, XrefRangeStart = 783083, XrefRangeEnd = 783093, MetadataInitTokenRva = 47221816L, MetadataInitFlagRva = 59849623L)]
	public unsafe void SetMix(string fromName, string toName, float duration)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(fromName);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(toName);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &duration;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMix_Public_Void_String_String_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783096, XrefRangeEnd = 783103, MetadataInitTokenRva = 47221792L, MetadataInitFlagRva = 59849624L)]
	public unsafe void SetMix(Animation from, Animation to, float duration)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)from);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)to);
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &duration;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMix_Public_Void_Animation_Animation_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 783103, XrefRangeEnd = 783108, MetadataInitTokenRva = 47221748L, MetadataInitFlagRva = 59849625L)]
	public unsafe float GetMix(Animation from, Animation to)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)from);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)to);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMix_Public_Single_Animation_Animation_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public AnimationStateData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
