from functools import reduce


# Get Critical Incidents
def get_critical_incidents(incidents):
    """
    Returns only incidents with severity = 'critical'
    """
    return list(filter(lambda i: i.severity == 'critical', incidents))


# Build Jira Payloads
def build_jira_payloads(incidents):
    """
    Converts incident objects to dictionaries (for API payload)
    """
    return list(map(lambda i: i.to_dict(), incidents))


# Count Incidents by Team
def count_by_team(incidents):
    """
    Returns dictionary:
    { team_name: count }
    """
    return reduce(
        lambda acc, i: {
            **acc,
            i.assigned_team: acc.get(i.assigned_team, 0) + 1
        },
        incidents,
        {}
    )


# Count by Severity
def count_by_severity(incidents):
    """
    Returns dictionary:
    { severity: count }
    """
    return reduce(
        lambda acc, i: {
            **acc,
            i.severity: acc.get(i.severity, 0) + 1
        },
        incidents,
        {}
    )


# Get Incidents by Type
def group_by_type(incidents):
    """
    Returns dictionary:
    { type: [incidents] }
    """
    result = {}

    for inc in incidents:
        result.setdefault(inc.type, []).append(inc)

    return result