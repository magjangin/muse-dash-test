using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSharpJson;

public class Lexer : Il2CppSystem.Object
{
	[OriginalName("spine-unity.dll", "", "Token")]
	public enum Token
	{
		None,
		Null,
		True,
		False,
		Colon,
		Comma,
		String,
		Number,
		CurlyOpen,
		CurlyClose,
		SquaredOpen,
		SquaredClose
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__lineNumber_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__parseNumbersAsFloat_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_json;

	private static readonly System.IntPtr NativeFieldInfoPtr_index;

	private static readonly System.IntPtr NativeFieldInfoPtr_success;

	private static readonly System.IntPtr NativeFieldInfoPtr_stringBuffer;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_hasError_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_lineNumber_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_lineNumber_Private_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_parseNumbersAsFloat_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Reset_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseString_Public_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetNumberString_Private_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseFloatNumber_Public_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ParseDoubleNumber_Public_Double_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLastIndexOfNumber_Private_Int32_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SkipWhiteSpaces_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LookAhead_Public_Token_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NextToken_Public_Token_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_NextToken_Private_Static_Token_Il2CppStructArray_1_Char_byref_Int32_0;

	public unsafe int _lineNumber_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lineNumber_k__BackingField);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__lineNumber_k__BackingField)) = num;
		}
	}

	public unsafe bool _parseNumbersAsFloat_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__parseNumbersAsFloat_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__parseNumbersAsFloat_k__BackingField)) = flag;
		}
	}

	public unsafe Il2CppStructArray<char> json
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_json);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<char>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_json)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int index
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_index);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_index)) = num;
		}
	}

	public unsafe bool success
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_success);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_success)) = flag;
		}
	}

	public unsafe Il2CppStructArray<char> stringBuffer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stringBuffer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<char>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stringBuffer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe bool hasError
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 781792, RefRangeEnd = 781793, XrefRangeStart = 781792, XrefRangeEnd = 781792, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_hasError_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int lineNumber
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 51188, RefRangeEnd = 51189, XrefRangeStart = 51188, XrefRangeEnd = 51189, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_lineNumber_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 80740, RefRangeEnd = 80748, XrefRangeStart = 80740, XrefRangeEnd = 80748, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_lineNumber_Private_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool parseNumbersAsFloat
	{
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_parseNumbersAsFloat_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static Lexer()
	{
		Il2CppClassPointerStore<Lexer>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "SharpJson", "Lexer");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Lexer>.NativeClassPtr);
		NativeFieldInfoPtr__lineNumber_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Lexer>.NativeClassPtr, "<lineNumber>k__BackingField");
		NativeFieldInfoPtr__parseNumbersAsFloat_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Lexer>.NativeClassPtr, "<parseNumbersAsFloat>k__BackingField");
		NativeFieldInfoPtr_json = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Lexer>.NativeClassPtr, "json");
		NativeFieldInfoPtr_index = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Lexer>.NativeClassPtr, "index");
		NativeFieldInfoPtr_success = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Lexer>.NativeClassPtr, "success");
		NativeFieldInfoPtr_stringBuffer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Lexer>.NativeClassPtr, "stringBuffer");
		NativeMethodInfoPtr_get_hasError_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663297);
		NativeMethodInfoPtr_get_lineNumber_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663298);
		NativeMethodInfoPtr_set_lineNumber_Private_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663299);
		NativeMethodInfoPtr_set_parseNumbersAsFloat_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663300);
		NativeMethodInfoPtr__ctor_Public_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663301);
		NativeMethodInfoPtr_Reset_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663302);
		NativeMethodInfoPtr_ParseString_Public_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663303);
		NativeMethodInfoPtr_GetNumberString_Private_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663304);
		NativeMethodInfoPtr_ParseFloatNumber_Public_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663305);
		NativeMethodInfoPtr_ParseDoubleNumber_Public_Double_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663306);
		NativeMethodInfoPtr_GetLastIndexOfNumber_Private_Int32_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663307);
		NativeMethodInfoPtr_SkipWhiteSpaces_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663308);
		NativeMethodInfoPtr_LookAhead_Public_Token_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663309);
		NativeMethodInfoPtr_NextToken_Public_Token_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663310);
		NativeMethodInfoPtr_NextToken_Private_Static_Token_Il2CppStructArray_1_Char_byref_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Lexer>.NativeClassPtr, 100663311);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781793, XrefRangeEnd = 781800, MetadataInitTokenRva = 47185452L, MetadataInitFlagRva = 59849593L)]
	public unsafe Lexer(string text)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Lexer>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(text);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void Reset()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Reset_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 781822, RefRangeEnd = 781823, XrefRangeStart = 781800, XrefRangeEnd = 781822, MetadataInitTokenRva = 47184584L, MetadataInitFlagRva = 59849594L)]
	public unsafe string ParseString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseString_Public_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 781826, RefRangeEnd = 781828, XrefRangeStart = 781823, XrefRangeEnd = 781826, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetNumberString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetNumberString_Private_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781828, XrefRangeEnd = 781834, MetadataInitTokenRva = 47184544L, MetadataInitFlagRva = 59849595L)]
	public unsafe float ParseFloatNumber()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseFloatNumber_Public_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781834, XrefRangeEnd = 781842, MetadataInitTokenRva = 47184520L, MetadataInitFlagRva = 59849596L)]
	public unsafe double ParseDoubleNumber()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ParseDoubleNumber_Public_Double_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(double*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781842, XrefRangeEnd = 781843, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe int GetLastIndexOfNumber(int index)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&index);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLastIndexOfNumber_Private_Int32_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(14)]
	[CachedScanResults(RefRangeStart = 781848, RefRangeEnd = 781862, XrefRangeStart = 781843, XrefRangeEnd = 781848, MetadataInitTokenRva = 47184860L, MetadataInitFlagRva = 59849597L)]
	public unsafe void SkipWhiteSpaces()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SkipWhiteSpaces_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781862, XrefRangeEnd = 781864, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Token LookAhead()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LookAhead_Public_Token_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Token*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 781864, XrefRangeEnd = 781866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Token NextToken()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NextToken_Public_Token_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Token*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 781866, RefRangeEnd = 781878, XrefRangeStart = 781866, XrefRangeEnd = 781866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Token NextToken(Il2CppStructArray<char> json, ref int index)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)json);
		*(void**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = Unsafe.AsPointer(ref index);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_NextToken_Private_Static_Token_Il2CppStructArray_1_Char_byref_Int32_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(Token*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public Lexer(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
