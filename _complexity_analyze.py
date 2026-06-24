import os, re, subprocess, sys

root = r"H:\source\repos\muse dash test"
files = subprocess.check_output(
    ["git", "ls-files", "muse dash test/*.cs"], cwd=root
).decode("utf-8").splitlines()

def scrub(src):
    """Replace string/char literal and comment content with spaces, preserving
    newlines and brace structure of real code (interpolated strings blanked
    entirely, removing their inner braces and code)."""
    out = []
    i, n = 0, len(src)
    while i < n:
        c = src[i]
        nxt = src[i+1] if i+1 < n else ""
        # line comment
        if c == "/" and nxt == "/":
            while i < n and src[i] != "\n":
                out.append(" "); i += 1
            continue
        # block comment
        if c == "/" and nxt == "*":
            out.append("  "); i += 2
            while i < n and not (src[i] == "*" and i+1 < n and src[i+1] == "/"):
                out.append("\n" if src[i] == "\n" else " "); i += 1
            out.append("  "); i += 2
            continue
        # verbatim / interpolated strings: @"...", $"...", $@"...", @$"..."
        if c in "@$" and (nxt == '"' or (nxt in "@$" and i+2 < n and src[i+2] == '"')):
            verbatim = "@" in (c + nxt)
            # advance to the opening quote
            out.append(" ")
            i += 1
            if src[i] in "@$":
                out.append(" "); i += 1
            # now src[i] == '"'
            out.append(" "); i += 1
            while i < n:
                if verbatim:
                    if src[i] == '"' and i+1 < n and src[i+1] == '"':
                        out.append("  "); i += 2; continue
                    if src[i] == '"':
                        out.append(" "); i += 1; break
                    out.append("\n" if src[i] == "\n" else " "); i += 1
                else:
                    if src[i] == "\\":
                        out.append("  "); i += 2; continue
                    if src[i] == '"':
                        out.append(" "); i += 1; break
                    out.append("\n" if src[i] == "\n" else " "); i += 1
            continue
        # regular string
        if c == '"':
            out.append(" "); i += 1
            while i < n:
                if src[i] == "\\":
                    out.append("  "); i += 2; continue
                if src[i] == '"':
                    out.append(" "); i += 1; break
                out.append("\n" if src[i] == "\n" else " "); i += 1
            continue
        # char literal
        if c == "'":
            out.append(" "); i += 1
            while i < n:
                if src[i] == "\\":
                    out.append("  "); i += 2; continue
                if src[i] == "'":
                    out.append(" "); i += 1; break
                out.append(" "); i += 1
            continue
        out.append(c); i += 1
    return "".join(out)

# method declaration: modifiers ... Name( ... )  followed (after balanced parens) by '{'
decl_re = re.compile(
    r'(?:(?:public|private|protected|internal|static|async|override|virtual|sealed|new|unsafe|extern|partial)\s+)+'
    r'[\w<>\[\],\s\.\?&]*?'           # return type (optional pieces)
    r'\b([A-Za-z_]\w*)\s*'            # method name (group 1)
    r'(?:<[\w\s,]+>)?\s*'            # generic params
    r'\('                             # open paren
)

KW = [r'\bif\b', r'\bfor\b', r'\bforeach\b', r'\bwhile\b', r'\bcase\b',
      r'\bcatch\b', r'&&', r'\|\|', r'\?\?']
kw_res = [re.compile(k) for k in KW]
ternary_re = re.compile(r'\s\?\s')

results = []
for rel in files:
    path = os.path.join(root, rel.replace("/", os.sep))
    try:
        with open(path, encoding="utf-8") as f:
            raw = f.read()
    except Exception:
        continue
    s = scrub(raw)
    for m in decl_re.finditer(s):
        name = m.group(1)
        # skip obvious control keywords mis-detected
        if name in ("if","for","foreach","while","switch","catch","using","lock","fixed","return","get","set"):
            continue
        # find end of param list (balanced parens) starting at the '(' we matched
        p = m.end() - 1
        depth = 0
        while p < len(s):
            if s[p] == "(":
                depth += 1
            elif s[p] == ")":
                depth -= 1
                if depth == 0:
                    p += 1
                    break
            p += 1
        # after params, skip whitespace / where-clauses to find '{' or ';' or '=>'
        q = p
        while q < len(s) and s[q] in " \t\r\n":
            q += 1
        # skip generic where constraints
        rest = s[q:q+200]
        if rest.startswith("where"):
            br = s.find("{", q)
            sc = s.find(";", q)
            if br == -1 or (sc != -1 and sc < br):
                continue
            q = br
        if q >= len(s) or s[q] != "{":
            continue  # abstract/interface/expression-bodied/no block
        # brace match the body
        bdepth = 0
        start = q
        j = q
        maxdepth = 0
        while j < len(s):
            if s[j] == "{":
                bdepth += 1
                maxdepth = max(maxdepth, bdepth)
            elif s[j] == "}":
                bdepth -= 1
                if bdepth == 0:
                    j += 1
                    break
            j += 1
        body = s[start:j]
        cc = 1
        for kr in kw_res:
            cc += len(kr.findall(body))
        cc += len(ternary_re.findall(body))
        loc = body.count("\n") + 1
        line_no = raw[:m.start()].count("\n") + 1
        results.append((cc, loc, maxdepth, name, rel, line_no))

# dedupe identical (name, file, line); rank by cc then loc
results.sort(key=lambda r: (r[0], r[1]), reverse=True)
print(f"{'CC':>4} {'LOC':>5} {'depth':>5}  name @ file:line")
print("-"*100)
for cc, loc, d, name, rel, ln in results[:20]:
    print(f"{cc:>4} {loc:>5} {d:>5}  {name} @ {rel}:{ln}")
