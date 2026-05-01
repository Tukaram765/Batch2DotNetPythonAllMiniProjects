import json
import os


class ReportGenerator:
    def __init__(self, incidents):
        self.incidents = incidents

    # Generate HTML Report
    def generate_html(self, output_file="output/report.html"):
        os.makedirs(os.path.dirname(output_file), exist_ok=True)

        html = f"""
        <html>
        <head>
            <title>IT Incident Report</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f6f9;
                    margin: 0;
                    padding: 0;
                }}

                .header {{
                    background-color: #2f4b7c;
                    color: white;
                    padding: 15px;
                    font-size: 20px;
                    font-weight: bold;
                }}

                .container {{
                    padding: 20px;
                }}

                .cards {{
                    display: flex;
                    gap: 15px;
                    margin-bottom: 20px;
                }}

                .card {{
                    background: white;
                    padding: 15px;
                    border-radius: 8px;
                    box-shadow: 0 2px 5px rgba(0,0,0,0.1);
                    flex: 1;
                    text-align: center;
                }}

                .card h2 {{
                    margin: 0;
                }}

                table {{
                    width: 100%;
                    border-collapse: collapse;
                    background: white;
                    border-radius: 8px;
                    overflow: hidden;
                }}

                th, td {{
                    padding: 10px;
                    text-align: left;
                    border-bottom: 1px solid #ddd;
                }}

                th {{
                    background-color: #2f4b7c;
                    color: white;
                }}

                .badge {{
                    padding: 5px 10px;
                    border-radius: 5px;
                    color: white;
                    font-size: 12px;
                }}

                .critical {{ background-color: red; }}
                .high {{ background-color: orange; }}
                .medium {{ background-color: gold; color: black; }}
                .low {{ background-color: green; }}

                .failed {{
                    color: red;
                    font-weight: bold;
                }}
            </style>
        </head>

        <body>

            <div class="header">
                IT Incident Auto-Triage Report
            </div>

            <div class="container">

                <!-- Summary Cards -->
                <div class="cards">
                    <div class="card">
                        <h2>{len(self.incidents)}</h2>
                        <p>Total Incidents</p>
                    </div>
                    <div class="card">
                        <h2>{len([i for i in self.incidents if i.severity == "critical"])}</h2>
                        <p>Critical</p>
                    </div>
                    <div class="card">
                        <h2>{len([i for i in self.incidents if i.severity == "high"])}</h2>
                        <p>High</p>
                    </div>
                    <div class="card">
                        <h2>{len([i for i in self.incidents if i.severity == "medium"])}</h2>
                        <p>Medium</p>
                    </div>
                    <div class="card">
                        <h2>{len([i for i in self.incidents if i.severity == "low"])}</h2>
                        <p>Low</p>
                    </div>
                </div>

                <!-- Table -->
                <table>
                    <tr>
                        <th>ID</th>
                        <th>Title</th>
                        <th>Type</th>
                        <th>Severity</th>
                        <th>Jira</th>
                        <th>ServiceNow</th>
                        <th>Azure</th>
                    </tr>
        """

        # Add rows
        for inc in self.incidents:
            severity_class = inc.severity

            jira = inc.ticket_ids.get("jira", "N/A")
            snow = inc.ticket_ids.get("snow", "N/A")
            azure = inc.ticket_ids.get("azure", "N/A")

            def format_ticket(val):
                return f'<span class="failed">{val}</span>' if val == "FAILED" else val

            html += f"""
                    <tr>
                        <td>{inc.id}</td>
                        <td>{inc.title}</td>
                        <td>{inc.type}</td>
                        <td><span class="badge {severity_class}">{inc.severity}</span></td>
                        <td>{format_ticket(jira)}</td>
                        <td>{format_ticket(snow)}</td>
                        <td>{format_ticket(azure)}</td>
                    </tr>
            """

        # Close HTML
        html += """
                </table>
            </div>
        </body>
        </html>
        """

        with open(output_file, "w", encoding="utf-8") as f:
            f.write(html)

        print("HTML report generated!")

    # Export JSON Summary
    def export_json(self, output_path="output/report.json"):
        os.makedirs(os.path.dirname(output_path), exist_ok=True)

        data = [inc.to_dict() for inc in self.incidents]

        with open(output_path, "w", encoding="utf-8") as f:
            json.dump(data, f, indent=4)

        print(f"JSON report generated at: {output_path}")
