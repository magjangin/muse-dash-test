using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SkeletonMecanim : SkeletonRenderer
{
	[System.Serializable]
	public class MecanimTranslator : Il2CppSystem.Object
	{
		public sealed class OnClipAppliedDelegate : Il2CppSystem.MulticastDelegate
		{
			private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

			private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_Animation_Int32_Single_Single_Single_Boolean_0;

			private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_Animation_Int32_Single_Single_Single_Boolean_AsyncCallback_Object_0;

			private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0;

			static OnClipAppliedDelegate()
			{
				Il2CppClassPointerStore<OnClipAppliedDelegate>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "OnClipAppliedDelegate");
				NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnClipAppliedDelegate>.NativeClassPtr, 100664147);
				NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_Animation_Int32_Single_Single_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnClipAppliedDelegate>.NativeClassPtr, 100664148);
				NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_Animation_Int32_Single_Single_Single_Boolean_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnClipAppliedDelegate>.NativeClassPtr, 100664149);
				NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Void_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<OnClipAppliedDelegate>.NativeClassPtr, 100664150);
			}

			[CallerCount(6192)]
			[CachedScanResults(RefRangeStart = 39733, RefRangeEnd = 45925, XrefRangeStart = 39733, XrefRangeEnd = 45925, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe OnClipAppliedDelegate(Il2CppSystem.Object @object, System.IntPtr method)
				: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<OnClipAppliedDelegate>.NativeClassPtr))
			{
				System.IntPtr* ptr = stackalloc System.IntPtr[2];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
				*(System.IntPtr**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &method;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			[CallerCount(1)]
			[CachedScanResults(RefRangeStart = 790596, RefRangeEnd = 790597, XrefRangeStart = 790561, XrefRangeEnd = 790596, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe virtual void Invoke(Animation clip, int layerIndex, float weight, float time, float lastTime, bool playsBackward)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[6];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)clip);
				*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerIndex;
				*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &weight;
				*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &time;
				*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &lastTime;
				*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &playsBackward;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_Animation_Int32_Single_Single_Single_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790597, XrefRangeEnd = 790609, MetadataInitTokenRva = 47129436L, MetadataInitFlagRva = 59848127L)]
			public unsafe virtual Il2CppSystem.IAsyncResult BeginInvoke(Animation clip, int layerIndex, float weight, float time, float lastTime, bool playsBackward, Il2CppSystem.AsyncCallback callback, Il2CppSystem.Object @object)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[8];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)clip);
				*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerIndex;
				*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &weight;
				*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &time;
				*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &lastTime;
				*(bool**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &playsBackward;
				*(System.IntPtr*)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)callback);
				*(System.IntPtr*)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)@object);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_Animation_Int32_Single_Single_Single_Boolean_AsyncCallback_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
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

			public OnClipAppliedDelegate(System.IntPtr pointer)
				: base(pointer)
			{
			}

			public static implicit operator OnClipAppliedDelegate(System.Action<Animation, int, float, float, float, bool> P_0)
			{
				return DelegateSupport.ConvertDelegate<OnClipAppliedDelegate>((System.Delegate)P_0);
			}

			public static OnClipAppliedDelegate operator +(OnClipAppliedDelegate P_0, OnClipAppliedDelegate P_1)
			{
				return ((Il2CppObjectBase)Il2CppSystem.Delegate.Combine(P_0, P_1)).Cast<OnClipAppliedDelegate>();
			}

			public static OnClipAppliedDelegate operator -(OnClipAppliedDelegate P_0, OnClipAppliedDelegate P_1)
			{
				object obj = Il2CppSystem.Delegate.Remove(P_0, P_1);
				if (obj != null)
				{
					obj = ((Il2CppObjectBase)obj).Cast<OnClipAppliedDelegate>();
				}
				return (OnClipAppliedDelegate)obj;
			}
		}

		[OriginalName("spine-unity.dll", "", "MixMode")]
		public enum MixMode
		{
			AlwaysMix,
			MixNext,
			Hard
		}

		public class ClipInfos : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr_isInterruptionActive;

			private static readonly System.IntPtr NativeFieldInfoPtr_isLastFrameOfInterruption;

			private static readonly System.IntPtr NativeFieldInfoPtr_clipInfoCount;

			private static readonly System.IntPtr NativeFieldInfoPtr_nextClipInfoCount;

			private static readonly System.IntPtr NativeFieldInfoPtr_interruptingClipInfoCount;

			private static readonly System.IntPtr NativeFieldInfoPtr_clipInfos;

			private static readonly System.IntPtr NativeFieldInfoPtr_nextClipInfos;

			private static readonly System.IntPtr NativeFieldInfoPtr_interruptingClipInfos;

			private static readonly System.IntPtr NativeFieldInfoPtr_stateInfo;

			private static readonly System.IntPtr NativeFieldInfoPtr_nextStateInfo;

			private static readonly System.IntPtr NativeFieldInfoPtr_interruptingStateInfo;

			private static readonly System.IntPtr NativeFieldInfoPtr_interruptingClipTimeAddition;

			private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

			public unsafe bool isInterruptionActive
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isInterruptionActive);
					return *(bool*)num;
				}
				set
				{
					*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isInterruptionActive)) = flag;
				}
			}

			public unsafe bool isLastFrameOfInterruption
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isLastFrameOfInterruption);
					return *(bool*)num;
				}
				set
				{
					*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_isLastFrameOfInterruption)) = flag;
				}
			}

			public unsafe int clipInfoCount
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipInfoCount);
					return *(int*)num;
				}
				set
				{
					*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipInfoCount)) = num;
				}
			}

			public unsafe int nextClipInfoCount
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextClipInfoCount);
					return *(int*)num;
				}
				set
				{
					*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextClipInfoCount)) = num;
				}
			}

			public unsafe int interruptingClipInfoCount
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interruptingClipInfoCount);
					return *(int*)num;
				}
				set
				{
					*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interruptingClipInfoCount)) = num;
				}
			}

			public unsafe List<AnimatorClipInfo> clipInfos
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipInfos);
					System.IntPtr intPtr = *(System.IntPtr*)num;
					return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<AnimatorClipInfo>>(intPtr) : null;
				}
				set
				{
					System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipInfos)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
				}
			}

			public unsafe List<AnimatorClipInfo> nextClipInfos
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextClipInfos);
					System.IntPtr intPtr = *(System.IntPtr*)num;
					return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<AnimatorClipInfo>>(intPtr) : null;
				}
				set
				{
					System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextClipInfos)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
				}
			}

			public unsafe List<AnimatorClipInfo> interruptingClipInfos
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interruptingClipInfos);
					System.IntPtr intPtr = *(System.IntPtr*)num;
					return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<AnimatorClipInfo>>(intPtr) : null;
				}
				set
				{
					System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
					IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interruptingClipInfos)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
				}
			}

			public unsafe AnimatorStateInfo stateInfo
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stateInfo);
					return *(AnimatorStateInfo*)num;
				}
				set
				{
					*(AnimatorStateInfo*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stateInfo)) = animatorStateInfo;
				}
			}

			public unsafe AnimatorStateInfo nextStateInfo
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextStateInfo);
					return *(AnimatorStateInfo*)num;
				}
				set
				{
					*(AnimatorStateInfo*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nextStateInfo)) = animatorStateInfo;
				}
			}

			public unsafe AnimatorStateInfo interruptingStateInfo
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interruptingStateInfo);
					return *(AnimatorStateInfo*)num;
				}
				set
				{
					*(AnimatorStateInfo*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interruptingStateInfo)) = animatorStateInfo;
				}
			}

			public unsafe float interruptingClipTimeAddition
			{
				get
				{
					nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interruptingClipTimeAddition);
					return *(float*)num;
				}
				set
				{
					*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_interruptingClipTimeAddition)) = num;
				}
			}

			static ClipInfos()
			{
				Il2CppClassPointerStore<ClipInfos>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "ClipInfos");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr);
				NativeFieldInfoPtr_isInterruptionActive = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "isInterruptionActive");
				NativeFieldInfoPtr_isLastFrameOfInterruption = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "isLastFrameOfInterruption");
				NativeFieldInfoPtr_clipInfoCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "clipInfoCount");
				NativeFieldInfoPtr_nextClipInfoCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "nextClipInfoCount");
				NativeFieldInfoPtr_interruptingClipInfoCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "interruptingClipInfoCount");
				NativeFieldInfoPtr_clipInfos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "clipInfos");
				NativeFieldInfoPtr_nextClipInfos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "nextClipInfos");
				NativeFieldInfoPtr_interruptingClipInfos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "interruptingClipInfos");
				NativeFieldInfoPtr_stateInfo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "stateInfo");
				NativeFieldInfoPtr_nextStateInfo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "nextStateInfo");
				NativeFieldInfoPtr_interruptingStateInfo = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "interruptingStateInfo");
				NativeFieldInfoPtr_interruptingClipTimeAddition = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, "interruptingClipTimeAddition");
				NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr, 100664151);
			}

			[CallerCount(3)]
			[CachedScanResults(RefRangeStart = 790626, RefRangeEnd = 790629, XrefRangeStart = 790609, XrefRangeEnd = 790626, MetadataInitTokenRva = 46420832L, MetadataInitFlagRva = 59848125L)]
			public unsafe ClipInfos()
				: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<ClipInfos>.NativeClassPtr))
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			public ClipInfos(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		public class AnimationClipEqualityComparer : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr_Instance;

			private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AnimationClip_AnimationClip_0;

			private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Final_New_Int32_AnimationClip_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

			public unsafe static IEqualityComparer<AnimationClip> Instance
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Instance, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEqualityComparer<AnimationClip>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Instance, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)equalityComparer));
				}
			}

			static AnimationClipEqualityComparer()
			{
				Il2CppClassPointerStore<AnimationClipEqualityComparer>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "AnimationClipEqualityComparer");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AnimationClipEqualityComparer>.NativeClassPtr);
				NativeFieldInfoPtr_Instance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AnimationClipEqualityComparer>.NativeClassPtr, "Instance");
				NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AnimationClip_AnimationClip_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationClipEqualityComparer>.NativeClassPtr, 100664152);
				NativeMethodInfoPtr_GetHashCode_Public_Virtual_Final_New_Int32_AnimationClip_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationClipEqualityComparer>.NativeClassPtr, 100664153);
				NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AnimationClipEqualityComparer>.NativeClassPtr, 100664154);
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe virtual bool Equals(AnimationClip x, AnimationClip y)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[2];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
				*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)y);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_AnimationClip_AnimationClip_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			[CallerCount(0)]
			[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe virtual int GetHashCode(AnimationClip o)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)o);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Final_New_Int32_AnimationClip_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			[CallerCount(2392)]
			[CachedScanResults(RefRangeStart = 18875, RefRangeEnd = 21267, XrefRangeStart = 18875, XrefRangeEnd = 21267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe AnimationClipEqualityComparer()
				: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AnimationClipEqualityComparer>.NativeClassPtr))
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			public AnimationClipEqualityComparer(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		public class IntEqualityComparer : Il2CppSystem.Object
		{
			private static readonly System.IntPtr NativeFieldInfoPtr_Instance;

			private static readonly System.IntPtr NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_Int32_Int32_0;

			private static readonly System.IntPtr NativeMethodInfoPtr_GetHashCode_Public_Virtual_Final_New_Int32_Int32_0;

			private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

			public unsafe static IEqualityComparer<int> Instance
			{
				get
				{
					Unsafe.SkipInit(out System.IntPtr intPtr);
					IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_Instance, (void*)(&intPtr));
					System.IntPtr intPtr2 = intPtr;
					return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEqualityComparer<int>>(intPtr2) : null;
				}
				set
				{
					IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_Instance, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)equalityComparer));
				}
			}

			static IntEqualityComparer()
			{
				Il2CppClassPointerStore<IntEqualityComparer>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "IntEqualityComparer");
				IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<IntEqualityComparer>.NativeClassPtr);
				NativeFieldInfoPtr_Instance = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<IntEqualityComparer>.NativeClassPtr, "Instance");
				NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntEqualityComparer>.NativeClassPtr, 100664156);
				NativeMethodInfoPtr_GetHashCode_Public_Virtual_Final_New_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntEqualityComparer>.NativeClassPtr, 100664157);
				NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<IntEqualityComparer>.NativeClassPtr, 100664158);
			}

			[CallerCount(0)]
			public unsafe virtual bool Equals(int x, int y)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[2];
				*ptr = (nint)(&x);
				*(int**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &y;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Equals_Public_Virtual_Final_New_Boolean_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			[CallerCount(0)]
			public unsafe virtual int GetHashCode(int o)
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = stackalloc System.IntPtr[1];
				*ptr = (nint)(&o);
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetHashCode_Public_Virtual_Final_New_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
			}

			[CallerCount(2392)]
			[CachedScanResults(RefRangeStart = 18875, RefRangeEnd = 21267, XrefRangeStart = 18875, XrefRangeEnd = 21267, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			public unsafe IntEqualityComparer()
				: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<IntEqualityComparer>.NativeClassPtr))
			{
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			}

			public IntEqualityComparer(System.IntPtr pointer)
				: base(pointer)
			{
			}
		}

		private static readonly System.IntPtr NativeFieldInfoPtr_autoReset;

		private static readonly System.IntPtr NativeFieldInfoPtr_useCustomMixMode;

		private static readonly System.IntPtr NativeFieldInfoPtr_layerMixModes;

		private static readonly System.IntPtr NativeFieldInfoPtr_layerBlendModes;

		private static readonly System.IntPtr NativeFieldInfoPtr__OnClipApplied;

		private static readonly System.IntPtr NativeFieldInfoPtr_animationTable;

		private static readonly System.IntPtr NativeFieldInfoPtr_clipNameHashCodeTable;

		private static readonly System.IntPtr NativeFieldInfoPtr_previousAnimations;

		private static readonly System.IntPtr NativeFieldInfoPtr_layerClipInfos;

		private static readonly System.IntPtr NativeFieldInfoPtr_animator;

		private static readonly System.IntPtr NativeMethodInfoPtr_add__OnClipApplied_Protected_add_Void_OnClipAppliedDelegate_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_remove__OnClipApplied_Protected_rem_Void_OnClipAppliedDelegate_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_add_OnClipApplied_Public_add_Void_OnClipAppliedDelegate_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_remove_OnClipApplied_Public_rem_Void_OnClipAppliedDelegate_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Void_Animator_SkeletonDataAsset_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ApplyAnimation_Private_Boolean_Skeleton_AnimatorClipInfo_AnimatorStateInfo_Int32_Single_MixBlend_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ApplyInterruptionAnimation_Private_Boolean_Skeleton_Boolean_AnimatorClipInfo_AnimatorStateInfo_Int32_Single_MixBlend_Single_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_OnClipAppliedCallback_Private_Void_Animation_AnimatorStateInfo_Int32_Single_Boolean_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Apply_Public_Void_Skeleton_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetActiveAnimationAndTime_Public_KeyValuePair_2_Animation_Single_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_AnimationTime_Private_Static_Single_Single_Single_Boolean_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_AnimationTime_Private_Static_Single_Single_Single_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_InitClipInfosForLayers_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_ClearClipInfosForLayers_Private_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetMixMode_Private_MixMode_Int32_MixBlend_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetStateUpdatesFromAnimator_Private_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetAnimatorClipInfos_Private_Void_Int32_byref_Boolean_byref_Int32_byref_Int32_byref_Int32_byref_IList_1_AnimatorClipInfo_byref_IList_1_AnimatorClipInfo_byref_IList_1_AnimatorClipInfo_byref_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetAnimatorStateInfos_Private_Void_Int32_byref_Boolean_byref_AnimatorStateInfo_byref_AnimatorStateInfo_byref_AnimatorStateInfo_byref_Single_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_GetAnimation_Private_Animation_AnimationClip_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		public unsafe bool autoReset
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_autoReset);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_autoReset)) = flag;
			}
		}

		public unsafe bool useCustomMixMode
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_useCustomMixMode);
				return *(bool*)num;
			}
			set
			{
				*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_useCustomMixMode)) = flag;
			}
		}

		public unsafe Il2CppStructArray<MixMode> layerMixModes
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_layerMixModes);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<MixMode>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_layerMixModes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe Il2CppStructArray<MixBlend> layerBlendModes
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_layerBlendModes);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<MixBlend>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_layerBlendModes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe OnClipAppliedDelegate _OnClipApplied
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__OnClipApplied);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<OnClipAppliedDelegate>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__OnClipApplied)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)onClipAppliedDelegate));
			}
		}

		public unsafe Dictionary<int, Animation> animationTable
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationTable);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<int, Animation>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animationTable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		public unsafe Dictionary<AnimationClip, int> clipNameHashCodeTable
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipNameHashCodeTable);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<AnimationClip, int>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_clipNameHashCodeTable)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
			}
		}

		public unsafe List<Animation> previousAnimations
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_previousAnimations);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Animation>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_previousAnimations)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
			}
		}

		public unsafe Il2CppReferenceArray<ClipInfos> layerClipInfos
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_layerClipInfos);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<ClipInfos>>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_layerClipInfos)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
			}
		}

		public unsafe Animator animator
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animator);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Animator>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_animator)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animator));
			}
		}

		static MecanimTranslator()
		{
			Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, "MecanimTranslator");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr);
			NativeFieldInfoPtr_autoReset = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "autoReset");
			NativeFieldInfoPtr_useCustomMixMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "useCustomMixMode");
			NativeFieldInfoPtr_layerMixModes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "layerMixModes");
			NativeFieldInfoPtr_layerBlendModes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "layerBlendModes");
			NativeFieldInfoPtr__OnClipApplied = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "_OnClipApplied");
			NativeFieldInfoPtr_animationTable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "animationTable");
			NativeFieldInfoPtr_clipNameHashCodeTable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "clipNameHashCodeTable");
			NativeFieldInfoPtr_previousAnimations = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "previousAnimations");
			NativeFieldInfoPtr_layerClipInfos = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "layerClipInfos");
			NativeFieldInfoPtr_animator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, "animator");
			NativeMethodInfoPtr_add__OnClipApplied_Protected_add_Void_OnClipAppliedDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664127);
			NativeMethodInfoPtr_remove__OnClipApplied_Protected_rem_Void_OnClipAppliedDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664128);
			NativeMethodInfoPtr_add_OnClipApplied_Public_add_Void_OnClipAppliedDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664129);
			NativeMethodInfoPtr_remove_OnClipApplied_Public_rem_Void_OnClipAppliedDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664130);
			NativeMethodInfoPtr_Initialize_Public_Void_Animator_SkeletonDataAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664131);
			NativeMethodInfoPtr_ApplyAnimation_Private_Boolean_Skeleton_AnimatorClipInfo_AnimatorStateInfo_Int32_Single_MixBlend_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664132);
			NativeMethodInfoPtr_ApplyInterruptionAnimation_Private_Boolean_Skeleton_Boolean_AnimatorClipInfo_AnimatorStateInfo_Int32_Single_MixBlend_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664133);
			NativeMethodInfoPtr_OnClipAppliedCallback_Private_Void_Animation_AnimatorStateInfo_Int32_Single_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664134);
			NativeMethodInfoPtr_Apply_Public_Void_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664135);
			NativeMethodInfoPtr_GetActiveAnimationAndTime_Public_KeyValuePair_2_Animation_Single_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664136);
			NativeMethodInfoPtr_AnimationTime_Private_Static_Single_Single_Single_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664137);
			NativeMethodInfoPtr_AnimationTime_Private_Static_Single_Single_Single_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664138);
			NativeMethodInfoPtr_InitClipInfosForLayers_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664139);
			NativeMethodInfoPtr_ClearClipInfosForLayers_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664140);
			NativeMethodInfoPtr_GetMixMode_Private_MixMode_Int32_MixBlend_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664141);
			NativeMethodInfoPtr_GetStateUpdatesFromAnimator_Private_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664142);
			NativeMethodInfoPtr_GetAnimatorClipInfos_Private_Void_Int32_byref_Boolean_byref_Int32_byref_Int32_byref_Int32_byref_IList_1_AnimatorClipInfo_byref_IList_1_AnimatorClipInfo_byref_IList_1_AnimatorClipInfo_byref_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664143);
			NativeMethodInfoPtr_GetAnimatorStateInfos_Private_Void_Int32_byref_Boolean_byref_AnimatorStateInfo_byref_AnimatorStateInfo_byref_AnimatorStateInfo_byref_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664144);
			NativeMethodInfoPtr_GetAnimation_Private_Animation_AnimationClip_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664145);
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr, 100664146);
		}

		[SpecialName]
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790629, XrefRangeEnd = 790633, MetadataInitTokenRva = 46344812L, MetadataInitFlagRva = 59848114L)]
		public unsafe void add__OnClipApplied(OnClipAppliedDelegate value)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add__OnClipApplied_Protected_add_Void_OnClipAppliedDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[SpecialName]
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790633, XrefRangeEnd = 790637, MetadataInitTokenRva = 46344864L, MetadataInitFlagRva = 59848115L)]
		public unsafe void remove__OnClipApplied(OnClipAppliedDelegate value)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove__OnClipApplied_Protected_rem_Void_OnClipAppliedDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[SpecialName]
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46344812L, MetadataInitFlagRva = 59848114L)]
		public unsafe void add_OnClipApplied(OnClipAppliedDelegate value)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_OnClipApplied_Public_add_Void_OnClipAppliedDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[SpecialName]
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46344864L, MetadataInitFlagRva = 59848115L)]
		public unsafe void remove_OnClipApplied(OnClipAppliedDelegate value)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_OnClipApplied_Public_rem_Void_OnClipAppliedDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 790657, RefRangeEnd = 790658, XrefRangeStart = 790637, XrefRangeEnd = 790657, MetadataInitTokenRva = 46344636L, MetadataInitFlagRva = 59848116L)]
		public unsafe void Initialize(Animator animator, SkeletonDataAsset skeletonDataAsset)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)animator);
			*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeletonDataAsset);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Void_Animator_SkeletonDataAsset_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 790674, RefRangeEnd = 790680, XrefRangeStart = 790658, XrefRangeEnd = 790674, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool ApplyAnimation(Skeleton skeleton, AnimatorClipInfo info, AnimatorStateInfo stateInfo, int layerIndex, float layerWeight, MixBlend layerBlendMode, bool useClipWeight1 = false)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[7];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
			*(AnimatorClipInfo**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &info;
			*(AnimatorStateInfo**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &stateInfo;
			*(int**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerIndex;
			*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerWeight;
			*(MixBlend**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerBlendMode;
			*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &useClipWeight1;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyAnimation_Private_Boolean_Skeleton_AnimatorClipInfo_AnimatorStateInfo_Int32_Single_MixBlend_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 790696, RefRangeEnd = 790699, XrefRangeStart = 790680, XrefRangeEnd = 790696, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool ApplyInterruptionAnimation(Skeleton skeleton, bool interpolateWeightTo1, AnimatorClipInfo info, AnimatorStateInfo stateInfo, int layerIndex, float layerWeight, MixBlend layerBlendMode, float interruptingClipTimeAddition, bool useClipWeight1 = false)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[9];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
			*(bool**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &interpolateWeightTo1;
			*(AnimatorClipInfo**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &info;
			*(AnimatorStateInfo**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &stateInfo;
			*(int**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerIndex;
			*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerWeight;
			*(MixBlend**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerBlendMode;
			*(float**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)))) = &interruptingClipTimeAddition;
			*(bool**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = &useClipWeight1;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyInterruptionAnimation_Private_Boolean_Skeleton_Boolean_AnimatorClipInfo_AnimatorStateInfo_Int32_Single_MixBlend_Single_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 790705, RefRangeEnd = 790707, XrefRangeStart = 790699, XrefRangeEnd = 790705, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void OnClipAppliedCallback(Animation clip, AnimatorStateInfo stateInfo, int layerIndex, float time, bool isLooping, float weight)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[6];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)clip);
			*(AnimatorStateInfo**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &stateInfo;
			*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerIndex;
			*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &time;
			*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &isLooping;
			*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &weight;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnClipAppliedCallback_Private_Void_Animation_AnimatorStateInfo_Int32_Single_Boolean_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 790804, RefRangeEnd = 790807, XrefRangeStart = 790707, XrefRangeEnd = 790804, MetadataInitTokenRva = 46344292L, MetadataInitFlagRva = 59848117L)]
		public unsafe void Apply(Skeleton skeleton)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Apply_Public_Void_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 790821, RefRangeEnd = 790822, XrefRangeStart = 790807, XrefRangeEnd = 790821, MetadataInitTokenRva = 46344408L, MetadataInitFlagRva = 59848118L)]
		public unsafe KeyValuePair<Animation, float> GetActiveAnimationAndTime(int layer)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&layer);
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetActiveAnimationAndTime_Public_KeyValuePair_2_Animation_Single_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new KeyValuePair<Animation, float>(pointer);
		}

		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 790823, RefRangeEnd = 790825, XrefRangeStart = 790822, XrefRangeEnd = 790823, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static float AnimationTime(float normalizedTime, float clipLength, bool loop, bool reversed)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[4];
			*ptr = (nint)(&normalizedTime);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clipLength;
			*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &loop;
			*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &reversed;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AnimationTime_Private_Static_Single_Single_Single_Boolean_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790825, XrefRangeEnd = 790826, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe static float AnimationTime(float normalizedTime, float clipLength, bool reversed)
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[3];
			*ptr = (nint)(&normalizedTime);
			*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &clipLength;
			*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &reversed;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AnimationTime_Private_Static_Single_Single_Single_Boolean_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790826, XrefRangeEnd = 790837, MetadataInitTokenRva = 46344580L, MetadataInitFlagRva = 59848119L)]
		public unsafe void InitClipInfosForLayers()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitClipInfosForLayers_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 790850, RefRangeEnd = 790851, XrefRangeStart = 790837, XrefRangeEnd = 790850, MetadataInitTokenRva = 46344392L, MetadataInitFlagRva = 59848120L)]
		public unsafe void ClearClipInfosForLayers()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearClipInfosForLayers_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 790851, RefRangeEnd = 790852, XrefRangeStart = 790851, XrefRangeEnd = 790851, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe MixMode GetMixMode(int layer, MixBlend layerBlendMode)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[2];
			*ptr = (nint)(&layer);
			*(MixBlend**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &layerBlendMode;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetMixMode_Private_MixMode_Int32_MixBlend_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(MixMode*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 790868, RefRangeEnd = 790869, XrefRangeStart = 790852, XrefRangeEnd = 790868, MetadataInitTokenRva = 46344524L, MetadataInitFlagRva = 59848121L)]
		public unsafe void GetStateUpdatesFromAnimator(int layer)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&layer);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetStateUpdatesFromAnimator_Private_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790869, XrefRangeEnd = 790873, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe void GetAnimatorClipInfos(int layer, out bool isInterruptionActive, out int clipInfoCount, out int nextClipInfoCount, out int interruptingClipInfoCount, out IList<AnimatorClipInfo> clipInfo, out IList<AnimatorClipInfo> nextClipInfo, out IList<AnimatorClipInfo> interruptingClipInfo, out bool shallInterpolateWeightTo1)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[9];
			*ptr = (nint)(&layer);
			*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isInterruptionActive);
			*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref clipInfoCount);
			*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref nextClipInfoCount);
			*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref interruptingClipInfoCount);
			byte* num = (byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)));
			nint num2 = 0;
			*(nint**)num = &num2;
			byte* num3 = (byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)));
			nint num4 = 0;
			*(nint**)num3 = &num4;
			byte* num5 = (byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(System.IntPtr)));
			nint num6 = 0;
			*(nint**)num5 = &num6;
			*(void**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref shallInterpolateWeightTo1);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAnimatorClipInfos_Private_Void_Int32_byref_Boolean_byref_Int32_byref_Int32_byref_Int32_byref_IList_1_AnimatorClipInfo_byref_IList_1_AnimatorClipInfo_byref_IList_1_AnimatorClipInfo_byref_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			nint num7 = num2;
			clipInfo = ((num7 == 0) ? null : new IList<AnimatorClipInfo>(num7));
			nint num8 = num4;
			nextClipInfo = ((num8 == 0) ? null : new IList<AnimatorClipInfo>(num8));
			nint num9 = num6;
			interruptingClipInfo = ((num9 == 0) ? null : new IList<AnimatorClipInfo>(num9));
		}

		[CallerCount(0)]
		public unsafe void GetAnimatorStateInfos(int layer, out bool isInterruptionActive, out AnimatorStateInfo stateInfo, out AnimatorStateInfo nextStateInfo, out AnimatorStateInfo interruptingStateInfo, out float interruptingClipTimeAddition)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[6];
			*ptr = (nint)(&layer);
			*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref isInterruptionActive);
			*(void**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref stateInfo);
			*(void**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref nextStateInfo);
			*(void**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref interruptingStateInfo);
			*(void**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref interruptingClipTimeAddition);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAnimatorStateInfos_Private_Void_Int32_byref_Boolean_byref_AnimatorStateInfo_byref_AnimatorStateInfo_byref_AnimatorStateInfo_byref_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 790881, RefRangeEnd = 790887, XrefRangeStart = 790873, XrefRangeEnd = 790881, MetadataInitTokenRva = 46344444L, MetadataInitFlagRva = 59848122L)]
		public unsafe Animation GetAnimation(AnimationClip clip)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)clip);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAnimation_Private_Animation_AnimationClip_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Animation>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790887, XrefRangeEnd = 790919, MetadataInitTokenRva = 46344756L, MetadataInitFlagRva = 59848123L)]
		public unsafe MecanimTranslator()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MecanimTranslator>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public MecanimTranslator(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_translator;

	private static readonly System.IntPtr NativeFieldInfoPtr_wasUpdatedAfterInit;

	private static readonly System.IntPtr NativeFieldInfoPtr__BeforeApply;

	private static readonly System.IntPtr NativeFieldInfoPtr__UpdateLocal;

	private static readonly System.IntPtr NativeFieldInfoPtr__UpdateWorld;

	private static readonly System.IntPtr NativeFieldInfoPtr__UpdateComplete;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Translator_Public_get_MecanimTranslator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add__BeforeApply_Protected_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove__BeforeApply_Protected_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add__UpdateLocal_Protected_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove__UpdateLocal_Protected_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add__UpdateWorld_Protected_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove__UpdateWorld_Protected_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add__UpdateComplete_Protected_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove__UpdateComplete_Protected_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_BeforeApply_Public_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_BeforeApply_Public_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_UpdateLocal_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_UpdateLocal_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_UpdateWorld_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_UpdateWorld_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_add_UpdateComplete_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_remove_UpdateComplete_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Virtual_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ApplyAnimation_Protected_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LateUpdate_Public_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe MecanimTranslator translator
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_translator);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MecanimTranslator>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_translator)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mecanimTranslator));
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
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BeforeApply)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe UpdateBonesDelegate _UpdateLocal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateLocal);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateLocal)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe UpdateBonesDelegate _UpdateWorld
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateWorld);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateWorld)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe UpdateBonesDelegate _UpdateComplete
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateComplete);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<UpdateBonesDelegate>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__UpdateComplete)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)updateBonesDelegate));
		}
	}

	public unsafe MecanimTranslator Translator
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Translator_Public_get_MecanimTranslator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MecanimTranslator>(intPtr) : null;
		}
	}

	static SkeletonMecanim()
	{
		Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SkeletonMecanim");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr);
		NativeFieldInfoPtr_translator = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, "translator");
		NativeFieldInfoPtr_wasUpdatedAfterInit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, "wasUpdatedAfterInit");
		NativeFieldInfoPtr__BeforeApply = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, "_BeforeApply");
		NativeFieldInfoPtr__UpdateLocal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, "_UpdateLocal");
		NativeFieldInfoPtr__UpdateWorld = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, "_UpdateWorld");
		NativeFieldInfoPtr__UpdateComplete = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, "_UpdateComplete");
		NativeMethodInfoPtr_get_Translator_Public_get_MecanimTranslator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664105);
		NativeMethodInfoPtr_add__BeforeApply_Protected_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664106);
		NativeMethodInfoPtr_remove__BeforeApply_Protected_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664107);
		NativeMethodInfoPtr_add__UpdateLocal_Protected_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664108);
		NativeMethodInfoPtr_remove__UpdateLocal_Protected_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664109);
		NativeMethodInfoPtr_add__UpdateWorld_Protected_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664110);
		NativeMethodInfoPtr_remove__UpdateWorld_Protected_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664111);
		NativeMethodInfoPtr_add__UpdateComplete_Protected_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664112);
		NativeMethodInfoPtr_remove__UpdateComplete_Protected_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664113);
		NativeMethodInfoPtr_add_BeforeApply_Public_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664114);
		NativeMethodInfoPtr_remove_BeforeApply_Public_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664115);
		NativeMethodInfoPtr_add_UpdateLocal_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664116);
		NativeMethodInfoPtr_remove_UpdateLocal_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664117);
		NativeMethodInfoPtr_add_UpdateWorld_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664118);
		NativeMethodInfoPtr_remove_UpdateWorld_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664119);
		NativeMethodInfoPtr_add_UpdateComplete_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664120);
		NativeMethodInfoPtr_remove_UpdateComplete_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664121);
		NativeMethodInfoPtr_Initialize_Public_Virtual_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664122);
		NativeMethodInfoPtr_Update_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664123);
		NativeMethodInfoPtr_ApplyAnimation_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664124);
		NativeMethodInfoPtr_LateUpdate_Public_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664125);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr, 100664126);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790919, XrefRangeEnd = 790923, MetadataInitTokenRva = 46273620L, MetadataInitFlagRva = 59848104L)]
	public unsafe void add__BeforeApply(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add__BeforeApply_Protected_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790923, XrefRangeEnd = 790927, MetadataInitTokenRva = 46273768L, MetadataInitFlagRva = 59848105L)]
	public unsafe void remove__BeforeApply(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove__BeforeApply_Protected_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790927, XrefRangeEnd = 790931, MetadataInitTokenRva = 46273692L, MetadataInitFlagRva = 59848106L)]
	public unsafe void add__UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add__UpdateLocal_Protected_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790931, XrefRangeEnd = 790935, MetadataInitTokenRva = 46273836L, MetadataInitFlagRva = 59848107L)]
	public unsafe void remove__UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove__UpdateLocal_Protected_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790935, XrefRangeEnd = 790939, MetadataInitTokenRva = 46273720L, MetadataInitFlagRva = 59848108L)]
	public unsafe void add__UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add__UpdateWorld_Protected_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790939, XrefRangeEnd = 790943, MetadataInitTokenRva = 46273876L, MetadataInitFlagRva = 59848109L)]
	public unsafe void remove__UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove__UpdateWorld_Protected_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790943, XrefRangeEnd = 790947, MetadataInitTokenRva = 46273636L, MetadataInitFlagRva = 59848110L)]
	public unsafe void add__UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add__UpdateComplete_Protected_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790947, XrefRangeEnd = 790951, MetadataInitTokenRva = 46273788L, MetadataInitFlagRva = 59848111L)]
	public unsafe void remove__UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove__UpdateComplete_Protected_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46273620L, MetadataInitFlagRva = 59848104L)]
	public unsafe void add_BeforeApply(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_BeforeApply_Public_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46273768L, MetadataInitFlagRva = 59848105L)]
	public unsafe void remove_BeforeApply(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_BeforeApply_Public_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46273692L, MetadataInitFlagRva = 59848106L)]
	public unsafe virtual void add_UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_UpdateLocal_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46273836L, MetadataInitFlagRva = 59848107L)]
	public unsafe virtual void remove_UpdateLocal(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_UpdateLocal_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46273720L, MetadataInitFlagRva = 59848108L)]
	public unsafe virtual void add_UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_UpdateWorld_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46273876L, MetadataInitFlagRva = 59848109L)]
	public unsafe virtual void remove_UpdateWorld(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_UpdateWorld_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46273636L, MetadataInitFlagRva = 59848110L)]
	public unsafe virtual void add_UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_add_UpdateComplete_Public_Virtual_Final_New_add_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[SpecialName]
	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 46273788L, MetadataInitFlagRva = 59848111L)]
	public unsafe virtual void remove_UpdateComplete(UpdateBonesDelegate value)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_remove_UpdateComplete_Public_Virtual_Final_New_rem_Void_UpdateBonesDelegate_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790951, XrefRangeEnd = 790991, MetadataInitTokenRva = 46273528L, MetadataInitFlagRva = 59848112L)]
	public unsafe override void Initialize(bool overwrite)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&overwrite);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Initialize_Public_Virtual_Void_Boolean_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790991, XrefRangeEnd = 790998, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Update_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 790998, XrefRangeEnd = 791005, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ApplyAnimation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ApplyAnimation_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791005, XrefRangeEnd = 791014, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void LateUpdate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_LateUpdate_Public_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 791014, XrefRangeEnd = 791053, MetadataInitTokenRva = 46273544L, MetadataInitFlagRva = 59848113L)]
	public unsafe SkeletonMecanim()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SkeletonMecanim>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SkeletonMecanim(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
