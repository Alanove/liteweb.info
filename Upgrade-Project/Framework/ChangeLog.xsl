<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<html>
			<style>
				*{
				font-family:arial;
				}
				table{
					width:100%;
				}
				table th{
				text-align:left;
				padding-bottom:10px;
				}
				table.main>tbody>tr>td{
				padding:20px;
				}
			</style>
			<body>
				<h1>Change Log</h1>
				<table border="1" class="main">
					<xsl:for-each select="ChangeLog/Change">
						<tr>
							<td>
								<b>Framework Version: </b>
								<xsl:value-of select="FrameworkVersion"/>
							</td>
						</tr>
						<tr>
							<td>
								<table>
									<tr>
										<th>Assembly</th>
										<th>Assembly Version</th>
										<th>Details</th>
									</tr>
									<xsl:for-each select="Changes">
										<tr>
											<td>
												<xsl:value-of select="Assembly"/>
											</td>
											<td>
												<xsl:value-of select="AssemblyVersion"/>
											</td>
											<td>
												<xsl:value-of select="Details"/>
											</td>
										</tr>
									</xsl:for-each>
								</table>
							</td>
						</tr>
						<tr>
							<TD>
								<b>Developer:</b>
								<xsl:value-of select="Developer"/>
								<BR />
								<b>Reviewed By:</b>
								<xsl:value-of select="ReviewedBy"/>
								<BR />
								<b>Date:</b>
								<xsl:value-of select="Date"/>
							</TD>
						</tr>
						<tr>
							<td>-</td></tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>