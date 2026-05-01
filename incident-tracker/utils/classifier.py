import re

# Pre-compiled Regex Patterns

# Incident type patterns
NETWORK_PATTERN = re.compile(
    r"\b(\d{1,3}(\.\d{1,3}){3}|tcp|udp|icmp|vlan|switch|firewall|dns|network)\b",
    re.IGNORECASE
)

SECURITY_PATTERN = re.compile(
    r"\b(breach|ransomware|brute[- ]?force|malware|phishing|unauthorized|attack)\b",
    re.IGNORECASE
)

APP_PATTERN = re.compile(
    r"\b(exception|error|http[- ]?\d{3}|nullpointerexception|stack trace|api)\b",
    re.IGNORECASE
)

# Severity patterns
CRITICAL_PATTERN = re.compile(
    r"\b(outage|down|breach|ransomware|production)\b",
    re.IGNORECASE
)

HIGH_PATTERN = re.compile(
    r"\b(timeout|failing|unavailable|unreachable|503)\b",
    re.IGNORECASE
)

MEDIUM_PATTERN = re.compile(
    r"\b(slow|degraded|warning|intermittent)\b",
    re.IGNORECASE
)


# Detect Incident Type

def detect_type(text: str) -> str:
    """
    Returns:
        'network', 'security', 'app', or 'general'
    """
    if not text:
        return "general"

    if SECURITY_PATTERN.search(text):
        return "security"
    elif NETWORK_PATTERN.search(text):
        return "network"
    elif APP_PATTERN.search(text):
        return "app"
    else:
        return "general"


# Detect Severity
def detect_severity(text: str) -> str:
    """
    Returns:
        'critical', 'high', 'medium', or 'low'
    """
    if not text:
        return "low"

    if CRITICAL_PATTERN.search(text):
        return "critical"
    elif HIGH_PATTERN.search(text):
        return "high"
    elif MEDIUM_PATTERN.search(text):
        return "medium"
    else:
        return "low"