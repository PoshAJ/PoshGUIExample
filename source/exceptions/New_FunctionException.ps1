function New_FunctionException {
    # Copyright (c) 2025 Anthony J. Raymond, MIT License (see manifest for details)
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseShouldProcessForStateChangingFunctions', '',
        Justification = 'Function does not change system state.')]

    [CmdletBinding()]
    [OutputType([System.Management.Automation.ErrorRecord])]
    param(
        [Parameter(
            Mandatory
        )]
        [ValidateNotNullOrEmpty()]
        [string] $Message,

        [Parameter()]
        [switch] $Throw = $false
    )

    ## LOGIC ###################################################################
    end {
        [System.Management.Automation.ErrorRecord] $ErrorRecord = [System.Management.Automation.ErrorRecord]::new(
            [Example.FunctionException] $Message,
            'Example.FunctionException',
            [System.Management.Automation.ErrorCategory]::InvalidOperation,
            $null
        )

        if ($Throw) {
            throw $ErrorRecord
        }

        $PSCmdlet.WriteObject($ErrorRecord)
    }
}
